using Microsoft.Extensions.Logging;

namespace Brew.Features.Patterns.Strategy;

/// <summary>
/// Credit Card payment strategy
/// </summary>
public class CreditCardPayment(ILogger<CreditCardPayment> logger) : IPaymentStrategy
{
    public string Name => "Credit Card";

    public void Execute(decimal amount)
    {
        logger.LogInformation("[Credit Card] Processing payment of ${Amount:F2}", amount);
        logger.LogInformation("[Credit Card] Validating card number...");
        logger.LogInformation("[Credit Card] Charging card...");
        logger.LogInformation("[Credit Card] Payment successful!");
    }
}