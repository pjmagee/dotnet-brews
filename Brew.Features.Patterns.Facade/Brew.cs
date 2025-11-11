using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brew.Features.Patterns.Facade;

/// <summary>
/// Demonstrates the Facade Pattern - providing a simplified interface to a complex subsystem
/// Use Case: Report generation requiring coordination of database, auth, and audit services
/// </summary>
public class Brew : ModuleBase
{
    protected override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        // Register subsystem services
        services.AddSingleton<ComplexServiceA>(); // Database
        services.AddSingleton<ComplexServiceB>(); // Authentication
        services.AddSingleton<ComplexServiceC>(); // Audit/Logging

        // Register facade
        services.AddSingleton<FacadeService>();

        // Register client services
        services.AddSingleton<ConsumingServiceBefore>();
        services.AddSingleton<ConsumingServiceAfter>();
    }

    protected override async Task ExecuteAsync(CancellationToken token = default)
    {
        Logger.LogInformation("=== FACADE PATTERN DEMONSTRATION ===\n");
        Logger.LogInformation("Scenario: Generating a report requires coordinating database, auth, and audit services\n");

        // BEFORE: Without Facade - Client must manage all subsystem complexity
        Logger.LogInformation("╔════════════════════════════════════════════════════════════╗");
        Logger.LogInformation("║  BEFORE: Without Facade (Complex, Tightly Coupled)        ║");
        Logger.LogInformation("╚════════════════════════════════════════════════════════════╝");
        
        var serviceBefore = Host.Services.GetRequiredService<ConsumingServiceBefore>();
        serviceBefore.GenerateReport();

        await Task.Delay(500); // Visual separation

        // AFTER: With Facade - Simple, clean interface
        Logger.LogInformation("╔════════════════════════════════════════════════════════════╗");
        Logger.LogInformation("║  AFTER: With Facade (Simple, Loosely Coupled)             ║");
        Logger.LogInformation("╚════════════════════════════════════════════════════════════╝");

        var serviceAfter = Host.Services.GetRequiredService<ConsumingServiceAfter>();
        serviceAfter.GenerateReport();

        // Summary
        Logger.LogInformation("═══════════════════════════════════════════════════════════");
        Logger.LogInformation("=== KEY BENEFITS OF FACADE PATTERN ===");
        Logger.LogInformation("═══════════════════════════════════════════════════════════");
        Logger.LogInformation("✓ SIMPLIFIED INTERFACE: One method instead of managing 9 method calls");
        Logger.LogInformation("✓ REDUCED COUPLING: Client depends on Facade, not all subsystem services");
        Logger.LogInformation("✓ ENCAPSULATION: Subsystem complexity hidden from client");
        Logger.LogInformation("✓ EASIER MAINTENANCE: Changes to subsystem don't affect clients");
        Logger.LogInformation("✓ IMPROVED READABILITY: facadeService.GenerateReport() is self-documenting");
        Logger.LogInformation("\nUse Cases: API clients, library wrappers, legacy system integration, microservices");
    }
}