using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brew.Features.IO.Channels.MessageBus;

internal sealed class QueueWriter(IEventBus bus, ILogger<QueueProcessor> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await bus.Publish(new HelloWorldEvent(), stoppingToken);
                    await Task.Delay(1000, stoppingToken);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error publishing event");
                }
            }
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("QueueWriter has been cancelled");
        }
    }
}