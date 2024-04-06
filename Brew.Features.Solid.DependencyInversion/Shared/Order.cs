namespace Brew.Features.Solid.DependencyInversion.Shared;

public class Order
{
    public int Id { get; set; }
    public decimal Price { get; set; }
    public List<string> Items { get; set; }
}