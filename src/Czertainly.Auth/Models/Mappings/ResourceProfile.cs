using AutoMapper;
using Czertainly.Auth.Models.Dto;
using Czertainly.Auth.Models.Entities;

namespace Czertainly.Auth.Models.Mappings
{
    public class ResourceProfile : Profile
    {
        public ResourceProfile()
        {
            CreateMap<ResourceRequestDto, Resource>();
            CreateMap<Resource, ResourceDto>();
            CreateMap<Resource, ResourceDetailDto>()
                .IncludeBase<Resource, ResourceDto>();
        }
    }
}
