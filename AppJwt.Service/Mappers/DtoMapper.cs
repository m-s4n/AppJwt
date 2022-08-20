using AppJwt.Core.Dtos;
using AppJwt.Core.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppJwt.Service.Mappers
{
    internal class DtoMapper : Profile
    {
        public DtoMapper()
        {
            CreateMap<UrunDto, Urun>().ReverseMap();
            CreateMap<UserDto, User>().ReverseMap();
        }
    }
}
