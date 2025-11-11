namespace Brew.Features.Solid.OpenClosed.After;

/// <summary>
/// FOLLOWS Open/Closed Principle:
/// - Define an abstraction that can be extended without modification
/// - New discount types can be added by creating new classes
/// - Existing code remains unchanged and untouched
/// </summary>
public interface IDiscountStrategy
{
    decimal CalculateDiscount(decimal orderTotal);
    string Description { get; }
}