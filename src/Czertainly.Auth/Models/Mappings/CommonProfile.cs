using AutoMapper;
using Czertainly.Auth.Common.Data;
using Czertainly.Auth.Common.Models.Dto;

namespace Czertainly.Auth.Models.Mappings
{
    public class CommonProfile : Profile
    {
        public CommonProfile()
        {
            CreateMap<IPagedList, PagingMetadata>();
            CreateMap<QueryRequestDto, QueryStringParameters>();
        }
    }
}
