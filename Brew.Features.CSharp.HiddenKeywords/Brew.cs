using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brew.Features.CSharp.HiddenKeywords;

/*
 * C# has several lesser-known keywords that provide low-level functionality:
 * 
 * __arglist: Variable-length argument lists (COM interop, rarely used)
 * __makeref, __refvalue, __reftype: TypedReference manipulation (unsafe, legacy)
 * stackalloc: Allocate memory on the stack (performance-critical scenarios)
 * sizeof: Get size of unmanaged types at compile time
 * 
 * These keywords exist for backwards compatibility and very specific scenarios.
 * Modern C# offers better alternatives (params, Span<T>, generics).
 */
public class Brew : ModuleBase
{
    protected override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        services.AddSingleton<ArgListDemo>();
        services.AddSingleton<TypedRefDemo>();
        services.AddSingleton<StackAllocDemo>();
        services.AddSingleton<SizeOfDemo>();
    }

    protected override Task ExecuteAsync(CancellationToken token = default)
    {
        Logger.LogInformation("=============== C# Hidden Keywords Demonstration ===============");
        Logger.LogInformation("Showcasing lesser-known keywords: __arglist, __makeref, stackalloc, sizeof");
        Logger.LogInformation("Note: Most are legacy/low-level; modern C# has better alternatives");
        Logger.LogInformation("=================================================================");
        Logger.LogInformation("");

        // __arglist demonstration
        Logger.LogInformation("--------------- __arglist (Variable Argument Lists) ------------");
        var argListDemo = Host.Services.GetRequiredService<ArgListDemo>();
        argListDemo.ModernApproach();
        argListDemo.LegacyApproach();
        Logger.LogInformation("");

        // TypedReference demonstration
        Logger.LogInformation("--------- __makeref/__refvalue/__reftype (TypedReference) ------");
        var typedRefDemo = Host.Services.GetRequiredService<TypedRefDemo>();
        typedRefDemo.LegacyRefKeywords();
        typedRefDemo.ModernAlternative();
        Logger.LogInformation("");

        // stackalloc demonstration
        Logger.LogInformation("--------------- stackalloc (Stack Allocation) -----------------");
        var stackAllocDemo = Host.Services.GetRequiredService<StackAllocDemo>();
        stackAllocDemo.HeapAllocation();
        stackAllocDemo.StackAllocation();
        Logger.LogInformation("");

        // sizeof demonstration
        Logger.LogInformation("------------------- sizeof (Type Sizes) -----------------------");
        var sizeOfDemo = Host.Services.GetRequiredService<SizeOfDemo>();
        sizeOfDemo.ShowTypeSizes();
        Logger.LogInformation("");

        Logger.LogInformation("========================== SUMMARY ============================");
        Logger.LogInformation("✓ __arglist: Use 'params' instead for variable arguments");
        Logger.LogInformation("✓ __makeref/etc: Use 'ref/out/in' for references");
        Logger.LogInformation("✓ stackalloc: Valid for performance (use Span<T> with it)");
        Logger.LogInformation("✓ sizeof: Useful for unmanaged structs and interop");
        Logger.LogInformation("===============================================================");

        return Task.CompletedTask;
    }
}