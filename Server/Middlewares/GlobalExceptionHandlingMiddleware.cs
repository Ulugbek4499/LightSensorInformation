using System.Net;
using System.Text.Json;
using MediatR;
using Server.Entities;
using Server.Notifications;

namespace Server.Middlewares
{
    public class GlobalExceptionHandlingMiddleware : IMiddleware
    {
        private readonly IMediator _mediator;

        public GlobalExceptionHandlingMiddleware(IMediator mediator)
        {
              _mediator = mediator;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await _mediator.Publish(new ExceptionNotification(DateTime.Now, ex));

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                ExceptionDetails details = new()
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Type = "Server Error",
                    Title = "Server Error",
                    Details = "Internal server error occurred"
                };


                context.Response.ContentType = "application/json";

                string json = JsonSerializer.Serialize(details);
                await context.Response.WriteAsync(json);
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
