using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brew.Features.IO.Channels.MessageBus;

internal sealed class QueueProcessor(InMemoryMessageQueue queue, ILogger<QueueProcessor> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var @event = await queue.Reader.ReadAsync(stoppingToken);
                    
                    if (@event is HelloWorldEvent e)
                    {
                        logger.LogInformation("Event received: {Event}", e.Message);    
                    }
                    else
                    {
                        logger.LogWarning("Unknown event received: {Event}", @event);
                    }
                }
                catch (OperationCanceledException)
                {
                    // Exit the loop on cancellation
                    break;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error processing event from queue");
                }
            }
            
            logger.LogInformation("QueueProcessor is shutting down");
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("QueueProcessor has been cancelled");
        }
    }
}