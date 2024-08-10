using Api.Models;
using ApiAdmin.Repository.Base;
using DTO.DTO;
using DTO.Event;
using HttpCall;
using System.Text.Json;

namespace ApiAdmin.Features.Empleados
{
    public class DarAltaEmpleadoNotification(IUnitOfWork _unitOfWork)
    {
        public async Task Notify(EmpleadoDTO empleado)
        {
            var eventDarAltaEmpleado = new RequestActivarEmpleado()
            {
                Correo = empleado.Correo,
                Cargo = empleado.Cargo,
                CodigoRH = empleado.CodigoRH
            };

            var backlogsEvent = new Backlogsevent()
            {
                CreateAt = DateTime.Now,
                EventType = (int)EventsEnum.DarAltaEmpleado,
                Json = JsonSerializer.Serialize(eventDarAltaEmpleado)
            };

            await _unitOfWork.BacklogsEvent.Add(backlogsEvent);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
