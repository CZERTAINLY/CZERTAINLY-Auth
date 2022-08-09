using AutoMapper;
using Czertainly.Auth.Common.Data;
using Czertainly.Auth.Common.Models.Dto;
using Czertainly.Auth.Common.Services;
using Czertainly.Auth.Data.Contracts;
using Czertainly.Auth.Models.Dto;
using Czertainly.Auth.Models.Entities;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace Czertainly.Auth.Services
{
    public class UserService : CrudService<User, UserDto, UserDetailDto>, IUserService
    {
        public UserService(IRepositoryManager repositoryManager, IMapper mapper): base(repositoryManager, repositoryManager.User, mapper)
        {
            
        }

        public async Task<UserProfileDto> GetUserProfileAsync(string certificate)
        {
            var clientCertificate = ParseCertificate(certificate);
            var isCertValid = VerifyClientCertificate(clientCertificate);
            


            throw new NotImplementedException();
        }

        public async Task<UserDetailDto> AssignRoleAsync(IEntityKey userKey, IEntityKey roleKey)
        {
            var user = await _repository.GetByKeyAsync(userKey);
            var role = await _repositoryManager.Role.GetByKeyAsync(roleKey);

            user.Roles.Add(role);
            await _repositoryManager.SaveAsync();

            return _mapper.Map<UserDetailDto>(user);
        }

        public async Task<UserDetailDto> AssignRolesAsync(IEntityKey userKey, IEnumerable<Guid> roleUuids)
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
            var decodedCertificate = HttpUtility.UrlDecode(certificate);
            var certPemString = decodedCertificate.Replace("-----BEGIN CERTIFICATE-----", "").Replace("-----END CERTIFICATE-----", "").ReplaceLineEndings("");
            return new X509Certificate2(Convert.FromBase64String(certPemString));
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
