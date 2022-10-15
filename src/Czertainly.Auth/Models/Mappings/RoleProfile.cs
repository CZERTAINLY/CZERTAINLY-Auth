using AutoMapper;
using Czertainly.Auth.Models.Dto;
using Czertainly.Auth.Models.Entities;

namespace Czertainly.Auth.Models.Mappings
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleRequestDto, Role>()
                .ForMember(x => x.Permissions, opt => opt.Ignore());
            CreateMap<RoleUpdateRequestDto, Role>();
            CreateMap<Role, RoleDto>();
            CreateMap<Role, RoleDetailDto>()
                .IncludeBase<Role, RoleDto>();
        }
    }
}
