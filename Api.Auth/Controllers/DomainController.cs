using ApiAuth.Models;
using ApiAuth.Repository.Base;
using DTO.Event;
using Microsoft.AspNetCore.Mvc;

namespace ApiAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DomainController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DomainController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("DarAltaUsuario")]
        public async Task<IActionResult> DarAltaUsuario(RequestActivarEmpleado request)
        {

            Thread.Sleep(2000);

            // Crear usuario en base de datos
            var entity = new Usuario()
            {
                Correo = request.Correo,
                Contrasenna = "123456",
                Role = request.Cargo,
                CodigoValidacion = "123456",
                ExpiracionCodigo = DateTime.Now,
                CodigoRh = request.CodigoRH,
                Status = false
            };

            await _unitOfWork.Usuario.Add(entity);
            await _unitOfWork.SaveChangesAsync();

            return Ok();
        }

    }
}
