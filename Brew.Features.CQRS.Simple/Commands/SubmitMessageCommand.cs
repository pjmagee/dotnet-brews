namespace Brew.Features.CQRS.Simple.Commands;

public class UpdateStockCommand(int productId, int quantityChange) : ICommand
{
    public int ProductId { get; } = productId;
    public int QuantityChange { get; } = quantityChange;
}
