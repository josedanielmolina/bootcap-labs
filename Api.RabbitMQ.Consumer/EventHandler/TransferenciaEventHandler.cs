using Api.RabbitMQ.Consumer.Models;
using Api.RabbitMQ.Consumer.RabbitMQ.Subscribe;
using DTO.Events;

namespace Api.RabbitMQ.Consumer.EventHandle
{
    public class TransferenciaEventHandler : IEventHandler<TransferenciaEvent>
    {
        private readonly ILogger<TransferenciaEventHandler> _logger;
        private readonly AppDbContext _context;

        public TransferenciaEventHandler(
            ILogger<TransferenciaEventHandler> logger,
            AppDbContext context
            )
        {
            _logger = logger;
            this._context = context;
        }

        public async Task Handle(TransferenciaEvent @event)
        {
            var transferencia = new Transferencia()
            {
                CuentaRemitente = @event.CuentaRemitente,
                CuentaDestino = @event.CuentaDestino,
                Monto = @event.Monto,
                Fecha = DateTime.Now
            };

            _context.Transferencias.Add(transferencia);
            await _context.SaveChangesAsync();
        }
    }
}
