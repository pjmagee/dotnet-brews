namespace Brew.Features.Solid.OpenClosed.After;

/// <summary>
/// Gold customer discount strategy - EXTENDS the system without modifying existing code
/// This is a NEW class that demonstrates the Open/Closed Principle
/// - No existing code was modified to add this feature
/// - The system is OPEN for extension
/// - Existing classes are CLOSED for modification
/// </summary>
public class GoldCustomerDiscount : IDiscountStrategy
{
    public string Description => "Gold Customer (15% discount)";
    
    public decimal CalculateDiscount(decimal orderTotal)
    {
        return orderTotal * 0.15m;
    }
}

