using MediatR;
using Serilog;

namespace Server.Notifications;
public record SaveTelemetryExcetionNotification(string deviceId, Exception ex) : INotification;

public class SaveTelemetryExcetionLogNotificationHandler : INotificationHandler<SaveTelemetryExcetionNotification>
{
    public Task Handle(SaveTelemetryExcetionNotification notification, CancellationToken cancellationToken)
    {
        Log.Information($"Error occurred on Save Telemetry Data ' deviceId: {notification.deviceId} ', and 'Message: {notification.ex.Message}'.");

        return Task.CompletedTask;
    }
}

public class SaveTelemetryExcetionConsoleNotificationHandler : INotificationHandler<SaveTelemetryExcetionNotification>
{
    public async Task Handle(SaveTelemetryExcetionNotification notification, CancellationToken cancellationToken)
    {
        await Console.Out.WriteLineAsync($"Error occurred on Save Telemetry Data ' deviceId: {notification.deviceId} ', and 'Message: {notification.ex.Message}'.");
    }
}
