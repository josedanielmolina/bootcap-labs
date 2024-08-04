using ApiAdmin.Models;
using AutoMapper;
using DTO.DTO.ApiAdmin;
using DTO.DTO.ApiAuth;


namespace ApiAdmin
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Empleado, EmpleadoDTO>().ReverseMap();
        }
    }
}
