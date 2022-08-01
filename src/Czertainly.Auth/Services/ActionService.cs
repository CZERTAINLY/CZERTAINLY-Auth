using AutoMapper;
using Czertainly.Auth.Common.Services;
using Czertainly.Auth.Data.Contracts;
using Czertainly.Auth.Models.Dto;
using Czertainly.Auth.Models.Entities;

namespace Czertainly.Auth.Services
{
    public class ActionService : CrudService<Models.Entities.Action, ActionDto, ActionDto>, IActionService
    {

        public ActionService(IRepositoryManager repositoryManager, IMapper mapper): base(repositoryManager, repositoryManager.Action, mapper)
        {
        }
    }
}
