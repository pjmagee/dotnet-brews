namespace Brew.Features.Solid.OpenClosed.Before;

/// <summary>
/// VIOLATES Open/Closed Principle:
/// - Every time we need to add a new discount type, we must MODIFY this class
/// - Adding new behavior requires changing existing, tested code
/// - Risk of breaking existing functionality when adding new features
/// - Class grows larger and more complex over time
/// </summary>
public class DiscountCalculator
{
    public decimal CalculateDiscount(string customerType, decimal orderTotal)
    {
        // Modification 1: Added regular customer discount
        if (customerType == "Regular")
        {
            return orderTotal * 0.05m; // 5% discount
        }

        // Modification 2: Added VIP customer discount
        if (customerType == "VIP")
        {
            return orderTotal * 0.10m; // 10% discount
        }

        // Modification 3: Added Gold customer discount
        if (customerType == "Gold")
        {
            return orderTotal * 0.15m; // 15% discount
        }

        // Every new customer type requires modifying this method!
        // What if we need Platinum? Diamond? Corporate?
        // This class is NOT closed for modification

        return 0;
    }
}