namespace Brew.Features.CQRS.Simple.Commands;

public class PrintMessageCommandHandler : ICommandHandler<PrintMessageCommand>
{
    public async Task HandleAsync(PrintMessageCommand command, CancellationToken cancellationToken)
    {
        // Simulate work
        await Task.Delay(500, cancellationToken); 
        Console.WriteLine($"Processed command: {command.Message}");
    }
}