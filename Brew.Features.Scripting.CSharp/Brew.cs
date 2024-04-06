using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Brew.Features.Scripting.CSharp;

public class Brew : ModuleBase
{
    protected override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        services.AddSingleton<IScriptRunner, ScriptRunner>();
        services.AddSingleton<ModuleBase>(this);
    }

    protected override async Task ExecuteAsync(CancellationToken token = default)
    {
        var runner = Host.Services.GetRequiredService<IScriptRunner>();
        await runner.RunScriptAsync();
            
        await runner.PerformOperation(10, 5, "+");
        await runner.PerformOperation(10, 5, "-");
        await runner.PerformOperation(10, 5, "*");
        await runner.PerformOperation(10, 5, "/");
    }
}