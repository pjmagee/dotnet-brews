namespace Brew.Features.InMemoryMessageBus;

internal class HelloWorldEvent : IIntegrationEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Message { get; } = "Hello, World!";
}