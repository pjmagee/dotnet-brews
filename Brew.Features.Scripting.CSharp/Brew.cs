using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Brew.Features.Scripting.CSharp;

public class Brew : ModuleBase
{
    protected override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        services.AddSingleton<ScriptRunnerDemo>();
        services.AddSingleton<ModuleBase>(this);
    }

    protected override async Task ExecuteAsync(CancellationToken token = default)
    {
        var runner = Host.Services.GetRequiredService<ScriptRunnerDemo>();
        
        // Execute hello script from file
        await runner.RunScriptAsync("hello.csx");
            
        // Execute calculator operations from file
        await runner.PerformOperation(10, 5, "+");
        await runner.PerformOperation(10, 5, "-");
        await runner.PerformOperation(10, 5, "*");
        await runner.PerformOperation(10, 5, "/");
    }
}