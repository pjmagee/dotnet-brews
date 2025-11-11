namespace Brew.Features.CQRS.Simple.Models;

// Write model: optimized for business logic and validation
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Stock { get; set; }
    public decimal Price { get; set; }
    public bool IsActive { get; set; } = true;
}

// Read model: denormalized view optimized for queries
public class ProductSummary
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Stock { get; set; }
    public decimal Price { get; set; }
    public bool IsActive { get; set; }
    public string StockStatus => Stock switch
    {
        <= 0 => "OUT_OF_STOCK",
        <= 10 => "LOW_STOCK",
        _ => "IN_STOCK"
    };
}
