using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brew.Features.DynamicRuntime.Dynamic;

/*
 * The 'dynamic' keyword enables runtime type resolution, bypassing compile-time type checking.
 * 
 * Use Cases:
 * - COM interop (Office automation)
 * - Working with dynamic languages (IronPython, IronRuby)
 * - JSON/XML deserialization without predefined types
 * - Reflection-heavy scenarios
 * 
 * Trade-offs:
 * - Loss of IntelliSense and compile-time safety
 * - Runtime errors instead of compile errors
 * - Performance overhead (uses DLR - Dynamic Language Runtime)
 * 
 * Prefer static typing when possible; use dynamic sparingly.
 */
public class Brew : ModuleBase
{
    protected override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        services.AddSingleton<ExpandoObjectDemo>();
        services.AddSingleton<DynamicVsStaticDemo>();
        services.AddSingleton<DynamicObjectDemo>();
    }

    protected override Task ExecuteAsync(CancellationToken token = default)
    {
        Logger.LogInformation("=============== Dynamic Runtime Demonstration ===============");
        Logger.LogInformation("Showcasing 'dynamic' keyword and ExpandoObject for runtime flexibility");
        Logger.LogInformation("WARNING: Use sparingly - sacrifices type safety for flexibility");
        Logger.LogInformation("==============================================================");
        Logger.LogInformation("");

        var expandoDemo = Host.Services.GetRequiredService<ExpandoObjectDemo>();
        expandoDemo.DemonstrateExpandoObject();
        Logger.LogInformation("");

        var staticDemo = Host.Services.GetRequiredService<DynamicVsStaticDemo>();
        staticDemo.CompareApproaches();
        Logger.LogInformation("");

        var dynamicObjDemo = Host.Services.GetRequiredService<DynamicObjectDemo>();
        dynamicObjDemo.ShowCustomDynamicBehavior();
        Logger.LogInformation("");

        Logger.LogInformation("======================= TRADE-OFFS =======================");
        Logger.LogInformation("✓ Flexibility: add members at runtime, duck typing");
        Logger.LogInformation("✗ Safety: no compile-time checks, runtime errors");
        Logger.LogInformation("✗ Performance: DLR overhead vs static dispatch");
        Logger.LogInformation("✗ Tooling: no IntelliSense, harder to refactor");
        Logger.LogInformation("RECOMMENDATION: Prefer static typing; use dynamic only when necessary");
        Logger.LogInformation("==========================================================");

        return Task.CompletedTask;
    }
}