using Api.Models;
using ApiAdmin.Repository.Base;
using AutoMapper;
using DTO.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadosController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmpleadosController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost("CreateEmpleado")]
        public async Task<IActionResult> CreateEmpleado(EmpleadoDTO empleado)
        {
            var entity = _mapper.Map<Empleado>(empleado);
            entity.CodigoRh = Guid.NewGuid().ToString();
            await _unitOfWork.EmpleadoRepository.Add(entity);
            await _unitOfWork.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("ActualizarEmpleado")]
        public async Task<IActionResult> ActualizarEmpleado(EmpleadoDTO dto)
        {

            var empleado = await _unitOfWork.EmpleadoRepository.GetSingleAsync(x => x.Id == dto.Id);

            if (empleado == null)
            {
                return BadRequest();
            }

            _mapper.Map(dto, empleado);

            _unitOfWork.EmpleadoRepository.Update(empleado);
            await _unitOfWork.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("GetEmpleados")]
        public async Task<IActionResult> GetEmpleados()
        {
            var empleados = await _unitOfWork.EmpleadoRepository.GetAsync();
            return Ok(_mapper.Map<List<EmpleadoDTO>>(empleados));
        }

        [HttpGet("GetEmpleadoByCodigoRH/{codigoRH}")]
        public async Task<IActionResult> GetEmpleados(string codigoRH)
        {
            var empleados = await _unitOfWork.EmpleadoRepository.GetSingleAsync(x => x.CodigoRh == codigoRH);
            return Ok(_mapper.Map<EmpleadoDTO>(empleados));
        }

        [HttpDelete("DeleteEmpleado/{empleadoId}")]
        public async Task<IActionResult> DeleteEmpleado(int empleadoId)
        {
            var empleado = await _unitOfWork.EmpleadoRepository.GetSingleAsync(x => x.Id == empleadoId);

            if (empleado == null)
            {
                return BadRequest("El empelado no existe");
            }

            _unitOfWork.EmpleadoRepository.Delete(empleado);
            await _unitOfWork.SaveChangesAsync();

            return Ok();
        }
    }
}
