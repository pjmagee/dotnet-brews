using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brew.Features.Mef;

/// <summary>
/// Demonstrates Managed Extensibility Framework (MEF) for plugin-based architecture with dynamic assembly loading.
/// Discovers calculator operations from plugin DLLs at runtime.
/// </summary>
public class Brew : ModuleBase
{
    protected override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        services.AddSingleton<CalculatorOperationsLoader>(x =>
        {
            var logger = x.GetRequiredService<ILogger<CalculatorOperationsLoader>>();
            var pluginDir = Directory.GetCurrentDirectory();
            return new CalculatorOperationsLoader(logger, pluginDir);
        });

        services.AddSingleton<Calculator>();
    }

    protected override Task ExecuteAsync(CancellationToken token = default)
    {
        Logger.LogInformation("========== MEF PLUGIN SYSTEM DEMONSTRATION ==========");
        Logger.LogInformation("Scenario: Dynamic calculator operations loaded from plugin assemblies\n");

        var calculator = Host.Services.GetRequiredService<Calculator>();

        Logger.LogInformation("---------- Discovered Operations ----------");
        var operations = calculator.Operatons.ToList();
        Logger.LogInformation("  Found {Count} plugin operations:", operations.Count);
        foreach (var op in operations)
        {
            Logger.LogInformation("    {Operation} ({Type})", op.Operation, op.GetType().Name);
        }

        Logger.LogInformation("\n---------- Executing Operations ----------");
        foreach (var op in operations)
        {
            calculator.Calculate(op, 10, 5);
        }

        Logger.LogInformation("\n---------- BENEFITS OF MEF ----------");
        Logger.LogInformation("✓ Plugin-based extensibility without recompilation");
        Logger.LogInformation("✓ Discovery via [Export]/[Import] attributes");
        Logger.LogInformation("✓ Supports lazy loading and lifetime management");
        Logger.LogInformation("✓ Ideal for modular/extensible applications");
        Logger.LogInformation("✓ Parts can be added/removed at runtime");

        return Task.CompletedTask;
    }
}
