using MediatR;
using Serilog;

namespace Server.Notifications
{
    public record ExceptionNotification(DateTime time, Exception ex, string userId) : INotification;

    public class ExceptionLogNotificationHandler : INotificationHandler<ExceptionNotification>
    {
        public Task Handle(ExceptionNotification notification, CancellationToken cancellationToken)
        {
            Log.Information($"Server Exception. 'Message: {notification.ex.Message}' 'Time: {notification.time}'.");

            return Task.CompletedTask;
        }
    }

    public class ExceptionConsoleNotificationHandler : INotificationHandler<ExceptionNotification>
    {
        public async Task Handle(ExceptionNotification notification, CancellationToken cancellationToken)
        {
            await Console.Out.WriteLineAsync(
                $"Server Exception. 'Message: {notification.ex.Message}' 'Time: {notification.time}'.");
        }
    }
}
