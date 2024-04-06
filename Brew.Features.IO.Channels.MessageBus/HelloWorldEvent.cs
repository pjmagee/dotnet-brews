namespace Brew.Features.IO.Channels.MessageBus;

internal class HelloWorldEvent : IIntegrationEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Message { get; } = "Hello, World!";
}