using Microsoft.Extensions.Logging;

namespace Brew.Features.Patterns.Strategy;

/// <summary>
/// Context class - allows runtime strategy swapping
/// This is the key to the Strategy pattern!
/// </summary>
public class Context(ILogger<Context> logger)
{
    private IPaymentStrategy? _strategy;

    /// <summary>
    /// Swap the strategy at runtime - this is the core of the Strategy pattern
    /// </summary>
    public void SetStrategy(IPaymentStrategy strategy)
    {
        logger.LogInformation("Switching payment method to: {StrategyName}", strategy.Name);
        _strategy = strategy;
    }

    public void ProcessPayment(decimal amount)
    {
        if (_strategy == null)
        {
            logger.LogError("No payment strategy set! Please set a strategy first.");
            return;
        }

        logger.LogInformation("Processing payment using {StrategyName} strategy...", _strategy.Name);
        _strategy.Execute(amount);
    }
}