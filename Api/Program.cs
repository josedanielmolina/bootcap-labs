using ApiAdmin;
using ApiAdmin.Features.Empleados;
using ApiAdmin.Repository.Base;
using Hangfire;
using Hangfire.MySql;
using HttpCall;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using Api.Models;

var builder = WebApplication.CreateBuilder(args);

IConfigurationRoot configuration = new ConfigurationBuilder()
  .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
  .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

builder.Host.UseSerilog(Log.Logger);

builder.Services.AddDbContext<AppDbContext>(
        (DbContextOptionsBuilder options) =>
        {
            options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddControllersWithViews().
        AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            options.JsonSerializerOptions.PropertyNamingPolicy = null;
        });

// Llamadas Http
builder.Services.AddHttpClient<IApiAuthHttpCall, ApiAuthHttpCall>(service =>
{
    service.BaseAddress = new Uri("https://localhost:7213/api/Domain/");
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Repository
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// UsesCases
builder.Services.AddScoped<CreateEmpleadoUseCase>();
builder.Services.AddScoped<DarAltaEmpleadoNotification>();

var connectionString = builder.Configuration.GetConnectionString("HangfireConnection");

//builder.Services.AddHangfire(config =>
//config.UseStorage(new MySqlStorage(connectionString, new MySqlStorageOptions
//{
//    TablesPrefix = "Hangfire"
//})));

//builder.Services.AddHangfireServer();

builder.Services.AddScoped<DarAltaEmpleadoJob>();

// Configuración de autenticación JWT
var jwtSettings = configuration.GetSection("JwtSettings");
var secretKey = jwtSettings.GetValue<string>("SecretKey");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.GetValue<string>("Issuer"),
        ValidAudience = jwtSettings.GetValue<string>("Audience"),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHangfireDashboard();
//var recurringJobManager = app.Services.GetRequiredService<IRecurringJobManager>();
//recurringJobManager.AddOrUpdate<DarAltaEmpleadoJob>("DarAltaEmpleadoJob", x => x.Execute(), Cron.Minutely);

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication(); // Añadir autenticación
app.UseAuthorization();

app.MapControllers();

app.Run();
