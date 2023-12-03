using MediatR;
using Serilog;

namespace Server.Notifications
{
    public record SaveTelemetryNotification(string comment, decimal amount) : INotification;

    public class SaveTelemetryLogNotificationHandler : INotificationHandler<SaveTelemetryNotification>
    {
        public Task Handle(SaveTelemetryNotification notification, CancellationToken cancellationToken)
        {
            Log.Information($"HomeBudget: New Income CREATED with comment: ' {notification.comment} ' and amount is: ' {notification.amount} '.");

            return Task.CompletedTask;
        }
    }

    public class SaveTelemetryConsoleNotificationHandler : INotificationHandler<SaveTelemetryNotification>
    {
        public async Task Handle(SaveTelemetryNotification notification, CancellationToken cancellationToken)
        {
            await Console.Out.WriteLineAsync($"HomeBudget: New Income CREATED with comment: ' {notification.comment} ' and amount is: ' {notification.amount} '.");
        }
    }
}
