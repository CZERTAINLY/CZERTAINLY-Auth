using AutoMapper;
using Czertainly.Auth.Common.Services;
using Czertainly.Auth.Data.Contracts;
using Czertainly.Auth.Models.Dto;
using Czertainly.Auth.Models.Entities;

namespace Czertainly.Auth.Services
{
    public class ResourceService : BaseResourceService<Resource, ResourceDto>, IResourceService
    {

        public ResourceService(IRepositoryManager repositoryManager, IMapper mapper): base(repositoryManager, repositoryManager.Resource, mapper)
        {
        }
    }
}
