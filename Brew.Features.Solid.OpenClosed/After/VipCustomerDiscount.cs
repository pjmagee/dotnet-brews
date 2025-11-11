namespace Brew.Features.Solid.OpenClosed.After;

/// <summary>
/// VIP customer discount strategy - EXTENDS the system without modifying existing code
/// </summary>
public class VipCustomerDiscount : IDiscountStrategy
{
    public string Description => "VIP Customer (10% discount)";
    
    public decimal CalculateDiscount(decimal orderTotal)
    {
        return orderTotal * 0.10m;
    }
}