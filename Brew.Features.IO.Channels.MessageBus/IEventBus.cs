namespace Brew.Features.IO.Channels.MessageBus;

internal interface IEventBus
{
    Task Publish<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : class, IIntegrationEvent;
}