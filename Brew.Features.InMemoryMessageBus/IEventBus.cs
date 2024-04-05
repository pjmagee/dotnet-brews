namespace Brew.Features.InMemoryMessageBus;

internal interface IEventBus
{
    Task Publish<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : class, IIntegrationEvent;
}