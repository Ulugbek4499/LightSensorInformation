using MediatR;
using Serilog;

namespace Server.Notifications
{
    public record GetStatisticsNotification(string deviceId, string userId) : INotification;

    public class GetStatisticsLogNotificationHandler : INotificationHandler<GetStatisticsNotification>
    {
        public Task Handle(GetStatisticsNotification notification, CancellationToken cancellationToken)
        {
            Log.Information($"Get Statistics Function Called: 'User Id: {notification.userId}' 'Device Id: {notification.deviceId}'");

            return Task.CompletedTask;
        }
    }

    public class GetStatisticsConsoleNotificationHandler : INotificationHandler<GetStatisticsNotification>
    {
        public async Task Handle(GetStatisticsNotification notification, CancellationToken cancellationToken)
        {
            await Console.Out.WriteLineAsync($"Get Statistics Function Called: 'User Id: {notification.userId}' 'Device Id: {notification.deviceId}'");
        }
    }
}
