using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;


namespace Api.RabbitMQ.Consumer.RabbitMQ.Subscribe
{
    public interface IEventHandler<in TEvent> where TEvent : class
    {
        Task Handle(TEvent @event);
    }
    public interface IMensajeriaSubscriber
    {
        void Subscribe<TEvent, TEventHandler>()
         where TEvent : class
         where TEventHandler : IEventHandler<TEvent>;
    }

    public class EventBusRabbitMQ : IMensajeriaSubscriber
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public EventBusRabbitMQ(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            var factory = new ConnectionFactory() { HostName = "localhost", UserName = "guest", Password = "guest" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void Subscribe<TEvent, TEventHandler>()
            where TEvent : class
            where TEventHandler : IEventHandler<TEvent>
        {
            var eventName = typeof(TEvent).Name;

            _channel.QueueDeclare(queue: eventName,
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var @event = JsonSerializer.Deserialize<TEvent>(message);

                bool messageHandled = false;

                using (var scope = _serviceProvider.CreateScope())
                {
                    var handler = scope.ServiceProvider.GetRequiredService<TEventHandler>();
                    try
                    {
                        await handler.Handle(@event);
                        messageHandled = true;
                    }
                    catch (Exception ex)
                    {
                        // Manejo del error
                        Console.WriteLine($"Error procesando el mensaje: {ex.Message}");
                    }
                }

                if (messageHandled)
                {
                    _channel.BasicConsume(eventName, true, consumer);
                }
                else
                {
                    // En caso de error, rechazar el mensaje para que pueda ser re-intentado
                    _channel.BasicNack(ea.DeliveryTag, false, true);
                }
            };

            _channel.BasicConsume(eventName, true, consumer);
        }
    }
}
