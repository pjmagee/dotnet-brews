using Brew.Features.Solid.OpenClosed.After;
using Brew.Features.Solid.OpenClosed.Before;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brew.Features.Solid.OpenClosed;

/// <summary>
/// Demonstrates the Open/Closed Principle.
/// 
/// OCP states: Software entities should be OPEN for extension but CLOSED for modification.
/// 
/// The discount calculator example:
/// - BEFORE: Adding new customer types requires modifying the DiscountCalculator class
/// - AFTER: New customer types are added by creating new IDiscountStrategy implementations
/// 
/// Benefits of following OCP:
/// 1. Existing code remains unchanged and stable
/// 2. Lower risk when adding new features
/// 3. Easier to test (test new strategies independently)
/// 4. Follows Single Responsibility Principle (each strategy has one reason to change)
/// </summary>
public class Brew : ModuleBase
{
    protected override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        // Register discount strategies
        services.AddSingleton<IDiscountStrategy, RegularCustomerDiscount>();
        services.AddSingleton<IDiscountStrategy, VipCustomerDiscount>();
        services.AddSingleton<IDiscountStrategy, GoldCustomerDiscount>();
    }

    protected override Task ExecuteAsync(CancellationToken token = default)
    {
        decimal orderTotal = 100.00m;

        Logger.LogInformation("=== BEFORE (VIOLATES OCP) ===");
        Logger.LogInformation("Every new customer type requires MODIFYING the DiscountCalculator class:");
        Logger.LogInformation("");

        var beforeCalculator = new DiscountCalculator();
        
        var regularDiscount = beforeCalculator.CalculateDiscount("Regular", orderTotal);
        Logger.LogInformation("Regular customer (order: ${OrderTotal}): Discount = ${Discount}", 
            orderTotal, regularDiscount);
        
        var vipDiscount = beforeCalculator.CalculateDiscount("VIP", orderTotal);
        Logger.LogInformation("VIP customer (order: ${OrderTotal}): Discount = ${Discount}", 
            orderTotal, vipDiscount);
        
        var goldDiscount = beforeCalculator.CalculateDiscount("Gold", orderTotal);
        Logger.LogInformation("Gold customer (order: ${OrderTotal}): Discount = ${Discount}", 
            orderTotal, goldDiscount);

        Logger.LogInformation("");
        Logger.LogInformation("✗ Adding 'Platinum' customer would require modifying DiscountCalculator");
        Logger.LogInformation("✗ Risk of breaking existing functionality");
        Logger.LogInformation("✗ Class grows more complex with each addition");
        Logger.LogInformation("");

        Logger.LogInformation("=== AFTER (FOLLOWS OCP) ===");
        Logger.LogInformation("New customer types are added by creating NEW classes:");
        Logger.LogInformation("");

        var strategies = Host.Services.GetServices<IDiscountStrategy>();
        
        foreach (var strategy in strategies)
        {
            var discount = strategy.CalculateDiscount(orderTotal);
            Logger.LogInformation("{Description} (order: ${OrderTotal}): Discount = ${Discount}", 
                strategy.Description, orderTotal, discount);
        }

        Logger.LogInformation("");
        Logger.LogInformation("✓ Add new customer types WITHOUT modifying existing code");
        Logger.LogInformation("✓ Each strategy is independently testable");
        Logger.LogInformation("✓ Existing code remains stable and unchanged");
        Logger.LogInformation("✓ System is OPEN for extension, CLOSED for modification");

        return Task.CompletedTask;
    }
}