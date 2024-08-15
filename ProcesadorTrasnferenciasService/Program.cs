using Api.RabbitMQ.Consumer.Models;
using DTO.Events;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Threading;

var context = new AppDbContext();
var factory = new ConnectionFactory() { HostName = "localhost", UserName = "guest", Password = "guest" };
var connection = factory.CreateConnection();
var channel = connection.CreateModel();

Console.WriteLine("Escuchando cola TransferenciaEvent....");
while (true)
{

    var eventName = typeof(TransferenciaEvent).Name;

    channel.QueueDeclare(queue: eventName,
                          durable: false,
                          exclusive: false,
                          autoDelete: false,
                          arguments: null);

    var consumer = new EventingBasicConsumer(channel);
    consumer.Received += async (model, ea) =>
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        var @event = JsonSerializer.Deserialize<TransferenciaEvent>(message);

        bool messageHandled = false;

        try
        {
            // Logica de negocio
            var transferencia = new Transferencia()
            {
                CuentaRemitente = @event.CuentaRemitente,
                CuentaDestino = @event.CuentaDestino,
                Monto = @event.Monto,
                Fecha = DateTime.Now
            };

            context.Transferencias.Add(transferencia);
            await context.SaveChangesAsync();
            messageHandled = true;
            Console.WriteLine("Evento procesado exitosamente");
        }
        catch (Exception ex)
        {
            // Manejo del error
            Console.WriteLine($"Error procesando el mensaje: {ex.Message}");
        }

        if (messageHandled)
        {
            channel.BasicConsume(eventName, true, consumer);
        }
        else
        {
            // En caso de error, rechazar el mensaje para que pueda ser re-intentado
            channel.BasicNack(ea.DeliveryTag, false, true);
        }
    };

    channel.BasicConsume(eventName, true, consumer);

    await Task.Delay(1000);
}