using AutoMapper;
using Czertainly.Auth.Common.Services;
using Czertainly.Auth.Data.Contracts;
using Czertainly.Auth.Models.Dto;

namespace Czertainly.Auth.Services
{
    public class ActionService : CrudService<Models.Entities.Action, ActionDto, ActionDto>, IActionService
    {

        public ActionService(IRepositoryManager repositoryManager, IMapper mapper, ILogger<ActionService> logger)
            : base(repositoryManager, repositoryManager.Action, mapper, logger)
        {
        }
    }
}
