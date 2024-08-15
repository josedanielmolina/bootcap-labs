using Api.RabbitMQ.Producer.RabbitMQ.Publish;
using DTO.Events;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.RabbitMQ.Producer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransferenciaController : ControllerBase
    {
        private readonly IMensajeriaPublisher _publisher;

        public TransferenciaController(IMensajeriaPublisher publisher)
        {
            _publisher = publisher;
        }

        [HttpPost("TransferirDinero")]
        public async Task<IActionResult> TransferirDinero(TransferenciaEvent @event)
        {
            _publisher.Publish(@event);
            return Ok("Transferencia realizada exitosamente");
        }


        [HttpPost("NotificarBySMS")]
        public async Task<IActionResult> NotificarBySMS(NotificarTransferenciaSMSEvent @event)
        {
            _publisher.Publish(@event);
            return Ok("SMS Enviado exitosamente");
        }
    }
}
