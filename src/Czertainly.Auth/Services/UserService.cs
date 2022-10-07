using AutoMapper;
using Czertainly.Auth.Common.Data;
using Czertainly.Auth.Common.Exceptions;
using Czertainly.Auth.Common.Models.Dto;
using Czertainly.Auth.Common.Services;
using Czertainly.Auth.Data.Contracts;
using Czertainly.Auth.Models.Config;
using Czertainly.Auth.Models.Dto;
using Czertainly.Auth.Models.Entities;
using Microsoft.Extensions.Options;
using System.Diagnostics;
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
            if (user.SystemUser) throw new InvalidActionException("Cannot update system user.");

            return await base.UpdateAsync(key, dto);
        }

        public override async Task DeleteAsync(Guid key)
        {
            var user = await _repository.GetByKeyAsync(key);
            if (user.SystemUser) throw new InvalidActionException("Cannot delete system user.");

            await base.DeleteAsync(key);
        }

        public async Task<AuthenticationResponseDto> AuthenticateUserAsync(AuthenticationRequestDto authenticationRequestDto)
        {
            User? user = null;
            if (!string.IsNullOrEmpty(authenticationRequestDto.CertificateContent))
            {
                var clientCertificate = ParseCertificate(authenticationRequestDto.CertificateContent);
                var isCertValid = VerifyClientCertificate(clientCertificate, out var chainStatusInfos);
                if (!isCertValid) throw new UnauthorizedException("User client certificate is invalid.", new Exception(string.Join('\n', chainStatusInfos)));

                var sha256Fingerprint = Convert.ToHexString(clientCertificate.GetCertHash(HashAlgorithmName.SHA256)).ToLower();

                user = await _repository.GetByConditionAsync(u => u.CertificateFingerprint == sha256Fingerprint);

                if (user == null) throw new UnauthorizedException("Unknown user for specified client certificate.");
            }
            else if (!string.IsNullOrEmpty(authenticationRequestDto.AuthenticationToken))
            {
                // Authentication token processing
                AuthenticationTokenDto? authenticationToken = null;
                try
                {
                    var decodedToken = Convert.FromBase64String(authenticationRequestDto.AuthenticationToken);
                    var decodedTokenString = Encoding.UTF8.GetString(decodedToken);
                    authenticationToken = JsonSerializer.Deserialize<AuthenticationTokenDto>(decodedToken);
                }
                catch (Exception ex)
                {
                    throw new UnauthorizedException("Wrong format of authentication token.", ex);
                }

                if (authenticationToken == null) throw new UnauthorizedException("Authentication token is empty or invalid JSON.");

                user = await _repository.GetByConditionAsync(u => u.Username == authenticationToken.Username);
                if (user == null && !_authOptions.CreateUnknownUsers) throw new UnauthorizedException($"Unknown user with username '{authenticationToken.Username}'.");

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
                    else throw new UnauthorizedException($"Unknown role '{authenticationToken.Roles}' for user with username '{authenticationToken.Username}'.");

                    await AssignRolesAsync(user.Uuid, roleUuid.HasValue ? new[] { roleUuid.Value } : Array.Empty<Guid>());
                }
                catch (UnauthorizedException) { throw; }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new UnauthorizedException("Error in creating user or assigning roles based on authentication token.", ex);
                }

                await transaction.CommitAsync();
            }
            else if (!string.IsNullOrEmpty(authenticationRequestDto.SystemUsername))
            {
                user = await _repository.GetByConditionAsync(u => u.SystemUser && u.Username == authenticationRequestDto.SystemUsername);

                if (user == null) throw new UnauthorizedException("Unknown system user for specified username: " + authenticationRequestDto.SystemUsername);
            }

            if (user == null) return new AuthenticationResponseDto { Authenticated = false };

            if (!user.Enabled) throw new UnauthorizedException($"User '{user.Username}' is disabled");

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
                throw new InvalidFormatException("Wrong format of user authentication certificate.", ex);
            }
        }

        private bool VerifyClientCertificate(X509Certificate2 certificate, out List<string> chainStatusInfos)
        {
            chainStatusInfos = new List<string>();

            var chain = new X509Chain();
            chain.ChainPolicy.VerificationFlags = X509VerificationFlags.NoFlag;
            chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck; // no revocation checking for now
            chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;

            var isValid = chain.Build(certificate);
            if (!isValid)
            {
                foreach (X509ChainElement chainElement in chain.ChainElements)
                {
                    foreach (X509ChainStatus chainStatus in chainElement.ChainElementStatus)
                    {
                        chainStatusInfos.Add($"Certificate issued by '{chainElement.Certificate.IssuerName.Name}' with subject '{chainElement.Certificate.SubjectName.Name}' is invalid: {chainStatus.StatusInformation}");
                    }
                }
            }
            return isValid;
        }

    }
}
