using MediatR;
using Serilog;

namespace Server.Notifications
{
    public record GetStatisticsNotification() : INotification;

    public class GetStatisticsLogNotificationHandler : INotificationHandler<GetStatisticsNotification>
    {
        public Task Handle(GetStatisticsNotification notification, CancellationToken cancellationToken)
        {
            Log.Information($"Get Statistics Function Called");

            return Task.CompletedTask;
        }
    }

    public class GetStatisticsConsoleNotificationHandler : INotificationHandler<GetStatisticsNotification>
    {
        public async Task Handle(GetStatisticsNotification notification, CancellationToken cancellationToken)
        {
            await Console.Out.WriteLineAsync($"Get Statistics Function Called");
        }
    }
}
