using System.Net;
using System.Text.Json;
using Talabat.APIs.Error;

namespace Talabat.APIs.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next , ILogger<ExceptionMiddleware> logger , IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }
        public async Task InvokeAsync(HttpContext context) 
        {
            try
            {
                // Befor next : Take an action with Request
                await _next.Invoke(context); // Go To next middleware
                // After next : Take an action with Response
            }
            catch (Exception ex)
            {
                // 1. Logg Exception
                _logger.LogError(ex.Message); // Development env
                // log exception in (File | Database ) => Production env

                // 2. Return Response of exception
                // Response Header
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                // Response Body [Send Custom Exception Response ]
                var response = _env.IsDevelopment() ? new ApiExceptionResponse(500, ex.Message, ex.StackTrace)
                    : new ApiExceptionResponse(500);

                var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                // convert to json
                var jsonResponse = JsonSerializer.Serialize(response , options);
                await context.Response.WriteAsync(jsonResponse); // write response in body
            }

        }
    }
}
