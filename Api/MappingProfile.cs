using Api.Empresa.Models;
using AutoMapper;
using DTO.DTO;


namespace ApiAdmin
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Empleado, EmpleadoDTO>().ReverseMap();
            CreateMap<Areasempresa, AreasempresaDTO>().ReverseMap();
        }
    }
}
