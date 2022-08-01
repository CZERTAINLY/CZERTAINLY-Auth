using AutoMapper;
using Czertainly.Auth.Models.Dto;

namespace Czertainly.Auth.Models.Mappings
{
    public class ActionProfile : Profile
    {
        public ActionProfile()
        {
            CreateMap<ActionRequestDto, Entities.Action>();
            CreateMap<Entities.Action, ActionDto>();
        }
    }
}
