using AutoMapper;
using Czertainly.Auth.Models.Dto;
using Endpoint = Czertainly.Auth.Models.Entities.Endpoint;

namespace Czertainly.Auth.Models.Mappings
{
    public class EndpointProfile : Profile
    {
        public EndpointProfile()
        {
            CreateMap<EndpointRequestDto, Endpoint>();
            CreateMap<Endpoint, EndpointDto>();
        }
    }
}
