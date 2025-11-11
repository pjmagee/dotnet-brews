using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brew.Features.Builders.FluentBuilder;

/// <summary>
/// Demonstrates fluent step-wise builder vs legacy manual sequencing.
/// </summary>
public class Brew : ModuleBase
{
    protected override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        services.AddTransient<FluentBuilder>();
    }

    protected override Task ExecuteAsync(CancellationToken token = default)
    {
        var logger = Host.Services.GetRequiredService<ILogger<Brew>>();
        logger.LogInformation("=== FLUENT BUILDER PATTERN DEMO ===\n");

        logger.LogInformation("❌ WITHOUT BUILDER (manual & error‑prone ordering):");
        var manualReport = new Report
        {
            Title = "Q4 Performance Summary",
            Footer = "Confidential"
        }; // Forgot sections first – easy to mis-order!
        manualReport.Sections.Add(("Executive Summary", "Growth up 25% year over year."));
        manualReport.Sections.Add(("Financials", "Revenue breakdown and cost analysis."));
        logger.LogInformation("Manual report built: {Report}", manualReport.ToString());
        logger.LogWarning("Ordering not enforced – footer set before sections; potential for partial objects.");

        logger.LogInformation("\n✓ WITH FLUENT STEP BUILDER (enforced order & readability):");
        var builder = Host.Services.GetRequiredService<FluentBuilder>();
        var fluentReport = builder
            .WithTitle("Q4 Performance Summary")
            .AddSection("Executive Summary", "Growth up 25% year over year.")
            .AddSection("Financials", "Revenue breakdown and cost analysis.")
            .AddSection("Roadmap", "Planned initiatives for next fiscal year.")
            .WithFooter("Confidential")
            .WithFooter("Confidential") // second interface hop returns build step
            .Build();
        logger.LogInformation("Fluent report built: {Report}", fluentReport.ToString());

        logger.LogInformation("\nKey Advantages:");
        logger.LogInformation("- Enforced order (cannot add footer before title)");
        logger.LogInformation("- Expressive chain reads like a DSL");
        logger.LogInformation("- Prevents partially initialised objects");
        logger.LogInformation("- Easier extension (add steps without breaking consumers)\n");

        return Task.CompletedTask;
    }
}