using MediatR;
using Serilog;

namespace Server.Notifications
{
    public record GetStatisticsExceptionNotification(string deviceId, Exception ex) : INotification;

    public class GetStatisticsExceptionLogNotificationHandler : INotificationHandler<GetStatisticsExceptionNotification>
    {
        public Task Handle(GetStatisticsExceptionNotification notification, CancellationToken cancellationToken)
        {
            Log.Information($"Error occurred on Getting Statistics. 'Device Id: {notification.deviceId}' 'Message: {notification.ex.Message}'.");

            return Task.CompletedTask;
        }
    }

    public class GetStatisticsExceptionConsoleNotificationHandler : INotificationHandler<GetStatisticsExceptionNotification>
    {
        public async Task Handle(GetStatisticsExceptionNotification notification, CancellationToken cancellationToken)
        {
            await Console.Out.WriteLineAsync($"Error occurred on Getting Statistics. 'Device Id: {notification.deviceId}' 'Message: {notification.ex.Message}'.");
        }
    }
}
