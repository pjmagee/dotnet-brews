namespace Brew.Features.CQRS.Simple.Commands;

public class PrintMessageCommand(string message) : ICommand
{
    public string Message { get; } = message;
}