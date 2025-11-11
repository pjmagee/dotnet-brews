namespace Brew.Features.CQRS.Simple.Commands;

public class DiscontinueProductCommand(int productId) : ICommand
{
    public int ProductId { get; } = productId;
}
