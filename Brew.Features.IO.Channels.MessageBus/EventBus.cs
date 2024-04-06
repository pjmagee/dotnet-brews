using Microsoft.Extensions.Logging;

namespace Brew.Features.IO.Channels.MessageBus;

internal class EventBus(ILogger<EventBus> logger, InMemoryMessageQueue messageQueue) : IEventBus
{
    public async Task Publish<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : class, IIntegrationEvent
    {
        await messageQueue.Writer.WriteAsync(@event, cancellationToken);
        logger.LogInformation("Event published: {Event}", System.Text.Json.JsonSerializer.Serialize(@event));
    }
}