namespace Brew.Features.Solid.OpenClosed.After;

/// <summary>
/// Regular customer discount strategy - EXTENDS the system without modifying existing code
/// </summary>
public class RegularCustomerDiscount : IDiscountStrategy
{
    public string Description => "Regular Customer (5% discount)";
    
    public decimal CalculateDiscount(decimal orderTotal)
    {
        return orderTotal * 0.05m;
    }
}