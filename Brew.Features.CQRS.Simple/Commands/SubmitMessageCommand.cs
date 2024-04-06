namespace Brew.Features.CQRS.Simple.Commands;

public class SubmitMessageCommand(string message) : ICommand
{
    public string Message { get; } = message;
}