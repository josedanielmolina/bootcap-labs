using Microsoft.AspNetCore.Connections;
using System.Text.Json;
using System.Text;
using RabbitMQ.Client;

namespace Api.RabbitMQ.Consumer.RabbitMQ.Publish
{
    public interface IMensajeriaPublisher
    {
        void Publish<T>(T @event) where T : class;
    }

    public class RabbitMqService : IMensajeriaPublisher
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMqService()
        {
            var factory = new ConnectionFactory() { HostName = "localhost", UserName = "guest", Password = "guest" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void Publish<T>(T @event) where T : class
        {
            var queueName = @event.GetType().Name;

            _channel.QueueDeclare(queue: queueName,
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);

            var message = JsonSerializer.Serialize(@event);
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "",
                                  routingKey: queueName,
                                  basicProperties: null,
                                  body: body);
        }

        ~RabbitMqService()
        {
            _channel?.Close();
            _connection?.Close();
        }
    }
}
