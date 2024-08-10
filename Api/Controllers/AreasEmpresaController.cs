using ApiAdmin.Repository.Base;
using AutoMapper;
using DTO.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AreasEmpresaController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AreasEmpresaController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("GetAreasEmpresa")]
        public async Task<IActionResult> GetAreasEmpresa()
        {
            var areas = await _unitOfWork.AreasempresaRepository.GetAsync();
            return Ok(_mapper.Map<List<AreasempresaDTO>>(areas));
        }
    }



}
