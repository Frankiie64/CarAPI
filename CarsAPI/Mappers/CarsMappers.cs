using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarsAPI.Models;
using CarsAPI.Models.Dtos;

namespace CarsAPI.Mappers
{
    public class CarsMappers : Profile
    {
        public CarsMappers()
        {
            CreateMap<Cars,CarsDtos>().ReverseMap();
        }
    }
}
