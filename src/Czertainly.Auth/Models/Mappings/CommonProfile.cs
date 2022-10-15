using AutoMapper;
using Czertainly.Auth.Common.Data;
using Czertainly.Auth.Common.Models.Dto;
using Czertainly.Auth.Common.Models.Entities;

namespace Czertainly.Auth.Models.Mappings
{
    public class CommonProfile : Profile
    {
        public CommonProfile()
        {
            CreateMap<IPagedList, PagingMetadata>();
            CreateMap<QueryRequestDto, QueryStringParameters>()
                .ForMember(dest => dest.SortBy, opt => opt.MapFrom(src => src.SortBy[0] == '-' ? char.ToUpper(src.SortBy[1]) + src.SortBy.Substring(2) : char.ToUpper(src.SortBy[0]) + src.SortBy.Substring(1)))
                .ForMember(dest => dest.SortAscending, opt => opt.MapFrom(src => src.SortBy[0] != '-'));

        }
    }
}
