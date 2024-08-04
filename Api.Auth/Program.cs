
using ApiAuth;
using ApiAuth.Models;
using ApiAuth.Repository.Base;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(
        (DbContextOptionsBuilder options) =>
        {
            options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

builder.Services.AddAutoMapper(typeof(MappingProfile));

// Llamadas Http
//builder.Services.AddHttpClient<IApiAuthService, ApiAuthService>(service =>
//{
//    service.BaseAddress = new Uri("https://localhost:7213/api/Domain/");
//});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Repository
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


