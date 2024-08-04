using ApiAdmin.Exceptions;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using Serilog.Context;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ApiAdmin.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger,
            IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ClientErrorException e)
            {
                _logger.LogError(e, "Client error occurred in {RequestMethod} {RequestPath}", context.Request.Method, context.Request.Path);
                _logger.LogError("Client Error Message: {Message}", e.Message);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                var response = new CodeErrorException((int)HttpStatusCode.BadRequest, e.Message);

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(response, options);

                await context.Response.WriteAsync(json);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception occurred in {RequestMethod} {RequestPath}", context.Request.Method, context.Request.Path);
                _logger.LogError("Exception Message: {Message}", e.Message);
                _logger.LogError("Exception StackTrace: {StackTrace}", e.StackTrace);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = _env.IsDevelopment()
                    ? new CodeErrorException((int)HttpStatusCode.InternalServerError, e.Message, e.StackTrace)
                    : new CodeErrorException((int)HttpStatusCode.InternalServerError, "Ha ocurrido un error, intente nuevamente o pongase en contacto con el proveedor del software");

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(response, options);

                await context.Response.WriteAsync(json);
            }
        }
    }

    public class CodeErrorException : CodeErrorResponse
    {
        [JsonIgnore]
        public string Detalle { get; set; }

        public CodeErrorException(int statusCode, string message = null, string details = null) : base(message)
        {
            Detalle = details;
        }
    }

    public class CodeErrorResponse
    {
        [JsonIgnore]
        public int CodigoHttp { get; set; }

        public string Mensaje { get; set; }

        public CodeErrorResponse(string message = null)
        {
            Mensaje = message;
        }

    }
}
