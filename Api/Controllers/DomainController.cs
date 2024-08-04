using ApiAdmin.Features.Empleados;
using DTO.DTO.ApiAdmin;
using Microsoft.AspNetCore.Mvc;

namespace ApiAdmin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DomainController : ControllerBase
    {
        private readonly CreateEmpleadoUseCase _createEmpleadoUseCase;

        public DomainController(
            CreateEmpleadoUseCase createEmpleadoUseCase
            )
        {
            _createEmpleadoUseCase = createEmpleadoUseCase;
        }

        [HttpPost("CreateEmpleadoUseCase")]
        public async Task<IActionResult> CreateEmpleadoUseCase(EmpleadoDTO empleado)
        {
            await _createEmpleadoUseCase.Execute(empleado);

            return Ok(new { Mensaje = "Felicidades tu primera peticion Http" });
        }
    }
}
