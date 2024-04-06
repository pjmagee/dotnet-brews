namespace Brew.Features.CQRS.Simple.Commands;

public class SubmitMessageCommandHandler(List<string> dataStore) : ICommandHandler<SubmitMessageCommand>
{
    public Task HandleAsync(SubmitMessageCommand command, CancellationToken cancellationToken)
    {
        dataStore.Add(command.Message);
        Console.WriteLine($"Message submitted: {command.Message}");
        return Task.CompletedTask;
    }
}