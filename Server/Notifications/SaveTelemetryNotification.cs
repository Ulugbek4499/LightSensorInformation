using MediatR;
using Serilog;

namespace Server.Notifications;
public record SaveTelemetryNotification(string deviceId, double illuminance, DateTime timeStamp) : INotification;

public class SaveTelemetryLogNotificationHandler : INotificationHandler<SaveTelemetryNotification>
{
    public Task Handle(SaveTelemetryNotification notification, CancellationToken cancellationToken)
    {
        Log.Information($"New TelemetryData Added: ' deviceId: {notification.deviceId} ', " +
            $"illuminance: ' {notification.illuminance} ' and 'timestamp: {notification.timeStamp}'.");

        return Task.CompletedTask;
    }
}

public class SaveTelemetryConsoleNotificationHandler : INotificationHandler<SaveTelemetryNotification>
{
    public async Task Handle(SaveTelemetryNotification notification, CancellationToken cancellationToken)
    {
        await Console.Out.WriteLineAsync($"New TelemetryData Added: ' deviceId: {notification.deviceId} '," +
            $" illuminance: ' {notification.illuminance} ' and 'timestamp: {notification.timeStamp}'.");
    }
}
