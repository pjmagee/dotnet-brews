namespace Brew.Features.Patterns.Strategy;

/// <summary>
/// Strategy interface - defines the contract for different payment algorithms
/// </summary>
public interface IPaymentStrategy
{
    string Name { get; }
    void Execute(decimal amount);
}