using Api.RabbitMQ.Consumer.EventHandle;
using Api.RabbitMQ.Consumer.Models;
using Api.RabbitMQ.Consumer.RabbitMQ.Publish;
using Api.RabbitMQ.Consumer.RabbitMQ.Subscribe;
using DTO.Events;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(
        (DbContextOptionsBuilder options) =>
        {
            options.UseMySQL(builder.Configuration["DatabaseContext"]);
        });

builder.Services.AddSingleton<IMensajeriaSubscriber, EventBusRabbitMQ>();
builder.Services.AddTransient<TransferenciaEventHandler>();

var app = builder.Build();

var eventBus = app.Services.GetRequiredService<IMensajeriaSubscriber>();
eventBus.Subscribe<TransferenciaEvent, TransferenciaEventHandler>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
