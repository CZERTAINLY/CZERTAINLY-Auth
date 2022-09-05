using AutoMapper;
using Czertainly.Auth.Common.Data;
using Czertainly.Auth.Common.Exceptions;
using Czertainly.Auth.Common.Models.Dto;
using Czertainly.Auth.Common.Services;
using Czertainly.Auth.Data.Contracts;
using Czertainly.Auth.Models.Dto;
using Czertainly.Auth.Models.Entities;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace Czertainly.Auth.Services
{
    public class UserService : CrudService<User, UserDto, UserDetailDto>, IUserService
    {
        private readonly IPermissionService _permissionService;

        public UserService(IRepositoryManager repositoryManager, IMapper mapper, IPermissionService permissionService): base(repositoryManager, repositoryManager.User, mapper)
        {
            _permissionService = permissionService;
        }

        public override async Task<UserDto> UpdateAsync(Guid key, ICrudRequestDto dto)
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

        public async Task<AuthenticationResponseDto> AuthenticateUserAsync(string certificate)
        {
            var clientCertificate = ParseCertificate(certificate);
            var isCertValid = VerifyClientCertificate(clientCertificate);
            var sha256Fingerprint = Convert.ToHexString(clientCertificate.GetCertHash(HashAlgorithmName.SHA256)).ToLower();

            var user = await _repository.GetByConditionAsync(u => u.CertificateFingerprint == sha256Fingerprint);
            if (user == null) return new AuthenticationResponseDto { Authenticated = false };

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

        public async Task<UserDetailDto> AssignRoleAsync(Guid userKey, Guid roleKey)
        {
            var user = await _repository.GetByKeyAsync(userKey);
            var role = await _repositoryManager.Role.GetByKeyAsync(roleKey);

            user.Roles.Add(role);
            await _repositoryManager.SaveAsync();

            return _mapper.Map<UserDetailDto>(user);
        }

        public async Task<UserDetailDto> AssignRolesAsync(Guid userKey, IEnumerable<Guid> roleUuids)
        {
            var user = await _repository.GetByKeyAsync(userKey);
            var roles = await _repositoryManager.Role.GetByUuidsAsync(roleUuids);

            user.Roles.Clear();
            foreach (var role in roles) user.Roles.Add(role);
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
