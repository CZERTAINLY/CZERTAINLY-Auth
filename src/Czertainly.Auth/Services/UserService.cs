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

        public UserService(IRepositoryManager repositoryManager, IMapper mapper, ILogger<UserService> logger, IOptions<AuthOptions> authOptions, IRoleService roleService, IPermissionService permissionService)
            : base(repositoryManager, repositoryManager.User, mapper, logger)
        {
            _authOptions = authOptions.Value;

            _roleService = roleService;
            _permissionService = permissionService;
        }

        public override async Task<UserDetailDto> CreateAsync(ICrudRequestDto dto)
        {
            var userRequestDto = dto as UserRequestDto;
            if (userRequestDto == null) throw new InvalidActionException("Cannot create user. Invalid DTO");

            // check uniqueness of user
            var checkedUser = await _repository.GetByConditionAsync(u => u.Username == userRequestDto.Username);
            if (checkedUser != null) throw new EntityNotUniqueException($"User with username '{userRequestDto.Username}' already exists");

            return await base.CreateAsync(dto);
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
                _logger.LogInformation("Authenticating user with certificate");
                var clientCertificate = ParseCertificate(authenticationRequestDto.CertificateContent);
                var isCertValid = VerifyClientCertificate(clientCertificate, out var chainStatusInfos);
                if (!isCertValid) throw new UnauthorizedException("User client certificate is invalid.", new Exception(string.Join('\n', chainStatusInfos)));

                var sha256Fingerprint = Convert.ToHexString(clientCertificate.GetCertHash(HashAlgorithmName.SHA256)).ToLower();
                _logger.LogInformation("Certificate parsed and verified. Fingerprint: " + sha256Fingerprint);

                user = await _repository.GetByConditionAsync(u => u.CertificateFingerprint == sha256Fingerprint);

                if (user == null) throw new UnauthorizedException("Unknown user for specified client certificate.");
            }
            else if (!string.IsNullOrEmpty(authenticationRequestDto.AuthenticationToken))
            {
                // Authentication token processing
                _logger.LogInformation("Authenticating user with OIDC token");
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
                var roleNames = authenticationToken.Roles ?? Array.Empty<string>();

                _logger.LogInformation($"Auth token contains user with username '{authenticationToken.Username}' and roles '{string.Join(',', roleNames)}'");

                user = await _repository.GetByConditionAsync(u => u.Username == authenticationToken.Username);
                if (user == null && !_authOptions.CreateUnknownUsers) throw new UnauthorizedException($"Unknown user with username '{authenticationToken.Username}'.");

                var transaction = await _repositoryManager.BeginTransactionAsync();

                try
                {
                    if (user == null)
                    {
                        _logger.LogInformation($"Creating new user with username '{authenticationToken.Username}'");
                        user = _mapper.Map<User>(authenticationToken);
                        _repository.Create(user);
                        await _repositoryManager.SaveAsync();
                    }

                    var userRoleNames = new HashSet<string>();
                    if (user.Roles != null) foreach (var role in user.Roles) userRoleNames.Add(role.Name);

                    foreach (var roleName in roleNames)
                    {
                        Guid? roleUuid = null;
                        var role = await _repositoryManager.Role.GetByConditionAsync(r => r.Name == roleName);

                        if (role != null && !userRoleNames.Contains(roleName)) roleUuid = role.Uuid;
                        if (role == null && _authOptions.CreateUnknownRoles)
                        {
                            _logger.LogInformation($"Creating new role with name '{roleName}'");
                            var roleDto = await _roleService.CreateAsync(new RoleRequestDto { Name = roleName });
                            roleUuid = roleDto.Uuid;
                        }

                        if (roleUuid.HasValue)
                        {
                            _logger.LogInformation($"Assign role '{roleName}' to user '{authenticationToken.Username}'");
                            await AssignRoleAsync(user.Uuid, roleUuid.Value);
                        }
                    }
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
                _logger.LogInformation($"Authenticating system user with username '{authenticationRequestDto.SystemUsername}'");
                user = await _repository.GetByConditionAsync(u => u.SystemUser && u.Username == authenticationRequestDto.SystemUsername);

                if (user == null) throw new UnauthorizedException("Unknown system user for specified username: " + authenticationRequestDto.SystemUsername);
            }

            if (user == null)
            {
                _logger.LogInformation("Authenticated as anonymous user");
                return new AuthenticationResponseDto { Authenticated = false };
            }

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

        public async Task<List<UserDto>> GetRoleUsersAsync(Guid roleUuid)
        {
            var users = await _repositoryManager.User.GetRoleUsersAsync(roleUuid);
            return _mapper.Map<List<UserDto>>(users);
        }
    }
}
