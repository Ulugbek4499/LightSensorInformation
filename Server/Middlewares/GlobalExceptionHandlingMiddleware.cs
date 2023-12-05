using System.Net;
using System.Security.Claims;
using System.Text.Json;
using MediatR;
using Server.Models;
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
                // Get the userId if the user is authenticated
                string userId = context.User.Identity?.IsAuthenticated == true
                      ? context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                      : null;


                // Publish ExceptionNotification with or without userId
                await _mediator.Publish(new ExceptionNotification(DateTime.Now, ex, userId));

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var errorResponse = new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Message = "Internal server error occurred"
                };

                context.Response.ContentType = "application/json";
                string json = JsonSerializer.Serialize(errorResponse);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
