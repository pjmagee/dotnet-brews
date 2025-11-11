using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brew.Features.Patterns.Strategy;

/// <summary>
/// Demonstrates the Strategy Pattern - allowing runtime swapping of algorithms
/// Use Case: Payment processing with different payment methods
/// </summary>
public class Brew : ModuleBase
{
    protected override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        services.AddSingleton<Context>();
        services.AddSingleton<CreditCardPayment>(); // Credit Card strategy
        services.AddSingleton<PayPalPayment>(); // PayPal strategy
        services.AddSingleton<CryptocurrencyStrategy>(); // Cryptocurrency strategy
    }

    protected override async Task ExecuteAsync(CancellationToken token = default)
    {
        Logger.LogInformation("=== STRATEGY PATTERN DEMONSTRATION ===\n");

        var context = Host.Services.GetRequiredService<Context>();
        var creditCardStrategy = Host.Services.GetRequiredService<CreditCardPayment>();
        var paypalStrategy = Host.Services.GetRequiredService<PayPalPayment>();
        var cryptoStrategy = Host.Services.GetRequiredService<CryptocurrencyStrategy>();

        Logger.LogInformation("--- Scenario: Processing Multiple Payments ---");
        Logger.LogInformation("Customer wants to make several purchases using different payment methods\n");

        // First purchase - using Credit Card
        Logger.LogInformation("=== Purchase 1: Book ($29.99) ===");
        context.SetStrategy(creditCardStrategy);
        context.ProcessPayment(29.99m);
        Logger.LogInformation("");

        await Task.Delay(100); // Simulate time between purchases

        // Second purchase - switching to PayPal at RUNTIME
        Logger.LogInformation("=== Purchase 2: Laptop ($1,299.99) ===");
        Logger.LogInformation("Customer decides to use PayPal for this larger purchase");
        context.SetStrategy(paypalStrategy); // RUNTIME SWAP!
        context.ProcessPayment(1299.99m);
        Logger.LogInformation("");

        await Task.Delay(100);

        // Third purchase - switching to Cryptocurrency at RUNTIME
        Logger.LogInformation("=== Purchase 3: Gaming PC ($2,499.99) ===");
        Logger.LogInformation("Customer wants to use Cryptocurrency for privacy");
        context.SetStrategy(cryptoStrategy); // RUNTIME SWAP to new strategy!
        context.ProcessPayment(2499.99m);
        Logger.LogInformation("");

        await Task.Delay(100);

        // Fourth purchase - switching back to Credit Card at RUNTIME
        Logger.LogInformation("=== Purchase 4: Coffee ($4.50) ===");
        Logger.LogInformation("Customer switches back to Credit Card for quick purchase");
        context.SetStrategy(creditCardStrategy); // RUNTIME SWAP AGAIN!
        context.ProcessPayment(4.50m);
        Logger.LogInformation("");

        Logger.LogInformation("=== Key Benefits of Strategy Pattern ===");
        Logger.LogInformation("✓ Strategies can be swapped at RUNTIME without changing Context code");
        Logger.LogInformation("✓ Each strategy encapsulates its own algorithm (different payment processing)");
        Logger.LogInformation("✓ Easy to add new strategies (e.g., CryptocurrencyStrategy) without modifying existing code");
        Logger.LogInformation("✓ Client code (Context) doesn't need to know implementation details");
        Logger.LogInformation("\nUse Cases: Payment processing, sorting algorithms, compression algorithms, routing strategies");
    }
}