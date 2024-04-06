namespace Brew.Features.IO.Channels.MessageBus;

internal interface IIntegrationEvent
{
    Guid Id { get; }
}