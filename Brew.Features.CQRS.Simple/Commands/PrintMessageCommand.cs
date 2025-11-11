namespace Brew.Features.CQRS.Simple.Commands;

public class AddProductCommand(int id, string name, int stock, decimal price) : ICommand
{
    public int Id { get; } = id;
    public string Name { get; } = name;
    public int Stock { get; } = stock;
    public decimal Price { get; } = price;
}
