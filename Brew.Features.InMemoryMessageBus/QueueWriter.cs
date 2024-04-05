using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brew.Features.InMemoryMessageBus;

internal sealed class QueueWriter(IEventBus bus, ILogger<QueueProcessor> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await bus.Publish(new HelloWorldEvent(), stoppingToken);
            await Task.Delay(1000, stoppingToken);
        }
    }
}