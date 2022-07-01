﻿using AutoMapper;
using Czertainly.Auth.Models.Dto;
using Czertainly.Auth.Models.Entities;

namespace Czertainly.Auth.Models.Mappings
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleRequestDto, Role>();
            CreateMap<Role, RoleDto>();
        }
    }
}
