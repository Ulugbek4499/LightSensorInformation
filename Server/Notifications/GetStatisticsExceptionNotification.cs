using MediatR;
using Serilog;

namespace Server.Notifications
{
    public record GetStatisticsExcetionNotification(string deviceId, Exception ex) : INotification;

    public class GetStatisticsExcetionLogNotificationHandler : INotificationHandler<GetStatisticsExcetionNotification>
    {
        public Task Handle(GetStatisticsExcetionNotification notification, CancellationToken cancellationToken)
        {
            Log.Information($"Error occurred on Save Telemetry Data ' deviceId: {notification.deviceId} ', and 'Message: {notification.ex.Message}'.");

            return Task.CompletedTask;
        }
    }

    public class GetStatisticsExcetionConsoleNotificationHandler : INotificationHandler<GetStatisticsExcetionNotification>
    {
        public async Task Handle(GetStatisticsExcetionNotification notification, CancellationToken cancellationToken)
        {
            await Console.Out.WriteLineAsync($"Error occurred on Save Telemetry Data ' deviceId: {notification.deviceId} ', and 'Message: {notification.ex.Message}'.");
        }
    }
}
