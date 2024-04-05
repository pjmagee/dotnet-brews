using Microsoft.Extensions.Logging;

namespace Brew.Features.InMemoryMessageBus;

internal class EventBus : IEventBus
{
    private readonly ILogger<EventBus> _logger;
    private readonly InMemoryMessageQueue _messageQueue;

    public EventBus(ILogger<EventBus> logger, InMemoryMessageQueue messageQueue)
    {
        _logger = logger;
        _messageQueue = messageQueue;
    }

    public async Task Publish<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : class, IIntegrationEvent
    {
        await _messageQueue.Writer.WriteAsync(@event, cancellationToken);
        _logger.LogInformation("Event published: {Event}", @event);
    }
}