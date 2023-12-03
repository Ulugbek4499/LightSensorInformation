using System.Net;
using System.Reflection;
using System.Text.Json;

namespace Server.Middlewares
{
        public class GlobalExceptionHandlingMiddleware : IMiddleware
        {
            private readonly ILogger _logger;

            public GlobalExceptionHandlingMiddleware(ILogger logger)
            {
                _logger = logger;
            }

            public async Task InvokeAsync(HttpContext context, RequestDelegate next)
            {
                try
                {
                    await next(context);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);

                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                    ExceptionDetails details = new()
                    {
                        Status=(int)HttpStatusCode.InternalServerError,
                        Type="Server Error",
                        Title="Server Error",
                        Details="Internal server error occurred"
                    };

                    string json=JsonSerializer.Serialize(details);
                    await context.Response.WriteAsync(json);
                    context.Response.ContentType = "application/json";

                }
            }
        }

        public class ExceptionDetails
        {
            public int Status { get; set; }
            public string Type { get; set; }
            public string Title { get; set; }
            public string Details { get; set; }
        }
}
