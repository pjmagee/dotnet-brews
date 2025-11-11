using Microsoft.Extensions.Logging;

namespace Brew.Features.Patterns.Strategy;

/// <summary>
/// Cryptocurrency payment strategy - demonstrates adding new strategies easily
/// </summary>
public class CryptocurrencyStrategy(ILogger<CryptocurrencyStrategy> logger) : IPaymentStrategy
{
    public string Name => "Cryptocurrency (Bitcoin)";

    public void Execute(decimal amount)
    {
        logger.LogInformation("[Cryptocurrency] Processing payment of ${Amount:F2}", amount);
        logger.LogInformation("[Cryptocurrency] Converting USD to BTC...");
        logger.LogInformation("[Cryptocurrency] Initiating blockchain transaction...");
        logger.LogInformation("[Cryptocurrency] Waiting for confirmation...");
        logger.LogInformation("[Cryptocurrency] Payment successful!");
    }
}
