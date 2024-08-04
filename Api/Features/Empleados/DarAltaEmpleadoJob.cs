using ApiAdmin.Repository.Base;
using DTO.DTO.ApiAdmin;
using DTO.Event;
using Hangfire;
using HttpCall;
using System.Text.Json;

namespace ApiAdmin.Features.Empleados
{
    public class DarAltaEmpleadoJob(
        IUnitOfWork _unitOfWork,
        IApiAuthHttpCall _apiAuthService,
        ILogger<DarAltaEmpleadoJob> _logger)
    {
        public async Task Execute()
        {

            var events = await _unitOfWork.BacklogsEvent
                .GetAsync(x => x.CompleteAt == null && x.EventType == (int)EventsEnum.DarAltaEmpleado);

            foreach (var item in events)
            {
                var @event = JsonSerializer.Deserialize<RequestActivarEmpleado>(item.Json);
                try
                {
                    var task = _apiAuthService.DarAltaUsuario(@event);
                    task.Wait(5000);

                    if (!task.IsCompleted || !task.Result)
                    {
                        _logger.LogError($"Erro al dar de alta el empleado con CodigoRH: {@event.CodigoRH}");
                        continue;
                    }

                    item.CompleteAt = DateTime.Now;
                    _unitOfWork.BacklogsEvent.Update(item);
                    await _unitOfWork.SaveChangesAsync();

                }
                catch (Exception)
                {
                    _logger.LogError($"Erro al dar de alta el empleado con CodigoRH: {@event.CodigoRH}");
                    continue;
                }
            }

        }
    }
}
