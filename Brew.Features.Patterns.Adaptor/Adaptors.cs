using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brew.Features.Patterns.Adaptor;

/// <summary>
/// Demonstrates the Adapter Pattern - making incompatible interfaces work together
/// Use Case: Integrating a legacy payment system into a modern application
/// </summary>
public class Adaptors : ModuleBase
{
    protected override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        // Register the legacy system
        services.AddSingleton<LegacyPaymentProcessor>();
        
        // Register the adapter that makes it compatible
        services.AddSingleton<IModernPaymentProcessor, PaymentProcessorAdapter>();
    }

    protected override async Task ExecuteAsync(CancellationToken token = default)
    {
        Logger.LogInformation("=== ADAPTER PATTERN DEMONSTRATION ===\n");
        Logger.LogInformation("Scenario: Integrating a legacy payment processor with incompatible interface\n");

        var legacyProcessor = Host.Services.GetRequiredService<LegacyPaymentProcessor>();
        var modernProcessor = Host.Services.GetRequiredService<IModernPaymentProcessor>();

        Logger.LogInformation("╔════════════════════════════════════════════════════════════╗");
        Logger.LogInformation("║  PROBLEM: Incompatible Interfaces                         ║");
        Logger.LogInformation("╚════════════════════════════════════════════════════════════╝\n");

        Logger.LogInformation("❌ BEFORE ADAPTER - Direct Legacy Usage (incompatible):");
        Logger.LogInformation("Legacy method signature: ProcessPaymentLegacy(string, string, string, decimal, string)");
        Logger.LogInformation("Legacy validation returns: int status code (0, 1, 2, 3)");
        Logger.LogInformation("Legacy status returns: string codes ('OK', 'FAIL', 'PEND')");
        Logger.LogInformation("Problem: Our modern code expects objects and booleans!\n");

        // Show what legacy system looks like
        var legacyTxId = legacyProcessor.ProcessPaymentLegacy("4111111111111111", "12/25", "123", 99.99m, "USD");
        Logger.LogInformation("Legacy call result: {TxId} (raw transaction ID, no structure)", legacyTxId);

        var legacyStatus = legacyProcessor.ValidateCardLegacy("4111111111111111");
        Logger.LogInformation("Legacy validation: {StatusCode} (what does this number mean?)\n", legacyStatus);

        await Task.Delay(500);

        Logger.LogInformation("╔════════════════════════════════════════════════════════════╗");
        Logger.LogInformation("║  SOLUTION: Adapter Pattern                                ║");
        Logger.LogInformation("╚════════════════════════════════════════════════════════════╝\n");

        Logger.LogInformation("✓ AFTER ADAPTER - Modern Interface (compatible):");
        Logger.LogInformation("Modern method signature: ProcessPayment(PaymentRequest)");
        Logger.LogInformation("Modern validation returns: bool (true/false)");
        Logger.LogInformation("Modern status returns: bool (true/false)");
        Logger.LogInformation("Solution: Adapter translates between the interfaces!\n");

        // Demonstrate adapter usage
        var requests = new[]
        {
            new PaymentRequest
            {
                CardNumber = "4111111111111111",
                ExpiryDate = "12/25",
                Cvv = "123",
                Amount = 99.99m,
                Currency = "USD"
            },
            new PaymentRequest
            {
                CardNumber = "5500000000000004",
                ExpiryDate = "06/26",
                Cvv = "456",
                Amount = 249.50m,
                Currency = "USD"
            }
        };

        foreach (var request in requests)
        {
            Logger.LogInformation("\n" + new string('=', 60));
            Logger.LogInformation("💳 Processing Payment:");
            Logger.LogInformation("   Card: {CardNumber}", request.CardNumber.Substring(0, 4) + "********" + request.CardNumber.Substring(12));
            Logger.LogInformation("   Amount: ${Amount:N2}", request.Amount);
            Logger.LogInformation(new string('=', 60));

            // Validate card using modern interface
            var isValid = modernProcessor.ValidateCard(request.CardNumber);
            Logger.LogInformation("\n[Application] Card validation result: {IsValid}", isValid ? "Valid ✓" : "Invalid ✗");

            if (isValid)
            {
                // Process payment using modern interface
                Logger.LogInformation("[Application] Processing payment through modern interface...\n");
                var result = modernProcessor.ProcessPayment(request);

                Logger.LogInformation("\n[Application] Payment Result:");
                Logger.LogInformation("   Success: {Success}", result.Success ? "Yes ✓" : "No ✗");
                Logger.LogInformation("   Transaction ID: {TransactionId}", result.TransactionId);
                Logger.LogInformation("   Message: {Message}", result.Message);

                // Check transaction status
                Logger.LogInformation("\n[Application] Verifying transaction status...\n");
                var isSuccessful = modernProcessor.IsTransactionSuccessful(result.TransactionId);
                Logger.LogInformation("[Application] Transaction successful: {IsSuccessful}\n", isSuccessful ? "Yes ✓" : "No ✗");
            }

            await Task.Delay(300);
        }

        Logger.LogInformation("\n" + new string('═', 60));
        Logger.LogInformation("=== KEY BENEFITS OF ADAPTER PATTERN ===");
        Logger.LogInformation(new string('═', 60));
        Logger.LogInformation("✓ COMPATIBILITY: Makes incompatible interfaces work together");
        Logger.LogInformation("✓ NO MODIFICATION: Legacy code remains unchanged");
        Logger.LogInformation("✓ CLEAN INTERFACE: Modern code uses clean, expected interface");
        Logger.LogInformation("✓ TRANSLATION: Adapter handles all format conversions");
        Logger.LogInformation("✓ TESTABILITY: Can mock modern interface for testing");
        Logger.LogInformation("✓ FLEXIBILITY: Easy to swap implementations");
        Logger.LogInformation("\nUse Cases: Legacy system integration, third-party API adaptation, interface standardization");
    }
}
