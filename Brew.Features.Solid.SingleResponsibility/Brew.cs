using Brew.Features.Solid.SingleResponsibility.After;
using Brew.Features.Solid.SingleResponsibility.Before;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brew.Features.Solid.SingleResponsibility;

/// <summary>
/// Demonstrates Single Responsibility Principle: each class should have one reason to change.
/// Contrasts a monolithic order class with a properly separated version.
/// </summary>
public class Brew : ModuleBase
{
    protected override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        services.AddSingleton<BeforeExample>().AddSingleton<OrderBefore>();

        services.AddSingleton<AfterExample>()
            .AddSingleton<OrderAfter>()
            .AddSingleton<Emailer>(x => new Emailer(x.GetRequiredService<ILogger<Emailer>>(), "smtp://localhost"))
            .AddSingleton<PaymentProcessor>()
            .AddSingleton<Checkout>();
    }

    protected override Task ExecuteAsync(CancellationToken token = default)
    {
        Logger.LogInformation("========== SINGLE RESPONSIBILITY PRINCIPLE (SRP) ==========");
        Logger.LogInformation("Scenario: Order checkout with payment and email notifications\n");

        Logger.LogInformation("---------- BEFORE (Violates SRP) ----------");
        Logger.LogInformation("  OrderBefore class handles: items, payment, email, persistence");
        var beforeExample = Host.Services.GetRequiredService<BeforeExample>();
        beforeExample.Execute();

        Logger.LogInformation("\n---------- AFTER (Follows SRP) ----------");
        Logger.LogInformation("  Responsibilities separated:");
        Logger.LogInformation("    - OrderAfter: manages order items");
        Logger.LogInformation("    - PaymentProcessor: handles payment logic");
        Logger.LogInformation("    - Emailer: sends notifications");
        Logger.LogInformation("    - Checkout: orchestrates the process");
        var afterExample = Host.Services.GetRequiredService<AfterExample>();
        afterExample.Execute();

        Logger.LogInformation("\n---------- BENEFITS OF SRP ----------");
        Logger.LogInformation("✓ Easier to understand and maintain");
        Logger.LogInformation("✓ Classes have one reason to change");
        Logger.LogInformation("✓ Better testability (isolated concerns)");
        Logger.LogInformation("✓ Promotes reusability");
        Logger.LogInformation("✓ Reduces coupling between components");

        return Task.CompletedTask;
    }
}
