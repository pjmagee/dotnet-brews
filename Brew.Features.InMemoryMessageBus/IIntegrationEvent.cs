namespace Brew.Features.InMemoryMessageBus;

internal interface IIntegrationEvent
{
    Guid Id { get; }
}