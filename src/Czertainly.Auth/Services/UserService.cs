﻿using AutoMapper;
using Czertainly.Auth.Common.Data;
using Czertainly.Auth.Common.Exceptions;
using Czertainly.Auth.Common.Models.Dto;
using Czertainly.Auth.Common.Services;
using Czertainly.Auth.Data.Contracts;
using Czertainly.Auth.Models.Config;
using Czertainly.Auth.Models.Dto;
using Czertainly.Auth.Models.Entities;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Web;

namespace Czertainly.Auth.Services
{
    public class UserService : CrudService<User, UserDto, UserDetailDto>, IUserService
    {
        private AuthOptions _authOptions;
        private readonly IRoleService _roleService;
        private readonly IPermissionService _permissionService;

        public UserService(IRepositoryManager repositoryManager, IMapper mapper, IOptions<AuthOptions> authOptions, IRoleService roleService, IPermissionService permissionService) : base(repositoryManager, repositoryManager.User, mapper)
        {
            _authOptions = authOptions.Value;

            _roleService = roleService;
            _permissionService = permissionService;
        }

        public override async Task<UserDetailDto> UpdateAsync(Guid key, ICrudRequestDto dto)
        {
            var user = await _repository.GetByKeyAsync(key);
            if (user.SystemUser) throw new Exception("Cannot update system user!");

            return await base.UpdateAsync(key, dto);
        }

        public override async Task DeleteAsync(Guid key)
        {
            var user = await _repository.GetByKeyAsync(key);
            if (user.SystemUser) throw new Exception("Cannot update system user!");

            await base.DeleteAsync(key);
        }

        public async Task<AuthenticationResponseDto?> AuthenticateUserAsync(AuthenticationRequestDto authenticationRequestDto)
        {
            if(string.IsNullOrEmpty(authenticationRequestDto.CertificateContent) && string.IsNullOrEmpty(authenticationRequestDto.AuthenticationToken)) return null;

            User? user = null;
            if (!string.IsNullOrEmpty(authenticationRequestDto.CertificateContent))
            {

                var clientCertificate = ParseCertificate(authenticationRequestDto.CertificateContent);
                var isCertValid = VerifyClientCertificate(clientCertificate);
                var sha256Fingerprint = Convert.ToHexString(clientCertificate.GetCertHash(HashAlgorithmName.SHA256)).ToLower();

                user = await _repository.GetByConditionAsync(u => u.CertificateFingerprint == sha256Fingerprint);
            }
            else if (!string.IsNullOrEmpty(authenticationRequestDto.AuthenticationToken))
            {
                // Authentication token processing
                var decodedToken = Convert.FromBase64String(authenticationRequestDto.AuthenticationToken);
                var decodedTokenString = Encoding.UTF8.GetString(decodedToken);

                var authenticationToken = JsonSerializer.Deserialize<AuthenticationTokenDto>(decodedToken);
                if (authenticationToken == null) return null;

                user = await _repository.GetByConditionAsync(u => u.Username == authenticationToken.Username);
                if (user == null && !_authOptions.CreateUnknownUsers) return null;

                var transaction = await _repositoryManager.BeginTransactionAsync();

                try
                {
                    if (user == null)
                    {
                        user = _mapper.Map<User>(authenticationToken);
                        _repository.Create(user);
                        await _repositoryManager.SaveAsync();
                    }

                    Guid? roleUuid = null;
                    var role = await _repositoryManager.Role.GetByConditionAsync(r => r.Name == authenticationToken.Roles);

                    if (role != null) roleUuid = role.Uuid;
                    else if (_authOptions.CreateUnknownRoles)
                    {
                        var roleDto = await _roleService.CreateAsync(new RoleRequestDto { Name = authenticationToken.Roles });
                        roleUuid = roleDto.Uuid;
                    }

                    await AssignRolesAsync(user.Uuid, roleUuid.HasValue ? new[] { roleUuid.Value } : Array.Empty<Guid>());
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    return null;
                }

                await transaction.CommitAsync();
            }
            else return new AuthenticationResponseDto { Authenticated = false };

            if (user == null) return null;

            var permissions = await _permissionService.GetUserPermissionsAsync(user.Uuid);

            var result = new AuthenticationResponseDto
            {
                Authenticated = true,
                Data = new UserProfileDto
                {
                    User = _mapper.Map<UserDto>(user),
                    Roles = user.Roles.Select(r => r.Name).ToList(),
                    Permissions = permissions,
                }
            };

            return result;
        }

        public async Task<UserDetailDto> EnableUserAsync(Guid userUuid, bool enableFlag)
        {
            var user = await _repository.GetByKeyAsync(userUuid);

            user.Enabled = enableFlag;
            await _repositoryManager.SaveAsync();

            return _mapper.Map<UserDetailDto>(user);
        }

        public async Task<UserDetailDto> AssignRoleAsync(Guid userUuid, Guid roleUuid)
        {
            var user = await _repository.GetByKeyAsync(userUuid);
            var role = await _repositoryManager.Role.GetByKeyAsync(roleUuid);

            user.Roles.Add(role);
            await _repositoryManager.SaveAsync();

            return _mapper.Map<UserDetailDto>(user);
        }

        public async Task<UserDetailDto> AssignRolesAsync(Guid userUuid, IEnumerable<Guid> roleUuids)
        {
            var user = await _repository.GetByKeyAsync(userUuid);
            var roles = await _repositoryManager.Role.GetByUuidsAsync(roleUuids);

            user.Roles.Clear();
            foreach (var role in roles) user.Roles.Add(role);
            await _repositoryManager.SaveAsync();

            return _mapper.Map<UserDetailDto>(user);
        }

        public async Task<UserDetailDto> RemoveRoleAsync(Guid userUuid, Guid roleUuid)
        {
            var user = await _repository.GetByKeyAsync(userUuid);
            var role = await _repositoryManager.Role.GetByKeyAsync(roleUuid);

            user.Roles.Remove(role);
            await _repositoryManager.SaveAsync();

            return _mapper.Map<UserDetailDto>(user);
        }

        private X509Certificate2 ParseCertificate(string clientCertificate)
        {
            var decodedCertificate = HttpUtility.UrlDecode(clientCertificate);
            var certPemString = decodedCertificate.Replace("-----BEGIN CERTIFICATE-----", "").Replace("-----END CERTIFICATE-----", "").ReplaceLineEndings("");

            try
            {
                return new X509Certificate2(Convert.FromBase64String(certPemString));
            }
            catch (FormatException ex)
            {
                throw new RequestException("Wrong format of user certificate: " + ex.Message);
            }
        }

        private bool VerifyClientCertificate(X509Certificate2 certificate)
        {
            var verify = new X509Chain();
            //verify.ChainPolicy.ExtraStore.Add(secureClient.CertificateAuthority); // add CA cert for verification
            verify.ChainPolicy.VerificationFlags = X509VerificationFlags.AllowUnknownCertificateAuthority; // this accepts too many certificates
            verify.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck; // no revocation checking
            verify.ChainPolicy.RevocationFlag = X509RevocationFlag.ExcludeRoot;
            return verify.Build(certificate);
        }

    }
}
