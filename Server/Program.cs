using System.Security.Claims;
using Server.Middlewares;
using Server.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServices(builder.Configuration);

SerilogService.SerilogSettings(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Use(async (context, next) =>
{
    context.Response.OnStarting(() =>
    {
        context.Response.Headers["UserId"] = "123";
        return Task.CompletedTask;
    });

    await next();
});
app.UseHttpLogging();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
app.MapControllers();

app.Run();
