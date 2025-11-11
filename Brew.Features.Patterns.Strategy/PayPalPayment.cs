using Microsoft.Extensions.Logging;

namespace Brew.Features.Patterns.Strategy;

/// <summary>
/// PayPal payment strategy
/// </summary>
public class PayPalPayment(ILogger<PayPalPayment> logger) : IPaymentStrategy
{
    public string Name => "PayPal";

    public void Execute(decimal amount)
    {
        logger.LogInformation("[PayPal] Processing payment of ${Amount:F2}", amount);
        logger.LogInformation("[PayPal] Authenticating with PayPal...");
        logger.LogInformation("[PayPal] Transferring funds...");
        logger.LogInformation("[PayPal] Payment successful!");
    }
}