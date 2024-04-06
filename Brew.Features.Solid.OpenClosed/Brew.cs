using Brew.Features.Solid.OpenClosed.After;
using Brew.Features.Solid.OpenClosed.Before;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Brew.Features.Solid.OpenClosed;

public class Brew : ModuleBase
{
    protected override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        services
            .AddSingleton<Worker>()
            .AddSingleton<FastWorker>()
            .AddSingleton<SlowWorker>();
    }

    protected override Task ExecuteAsync(CancellationToken token = default)
    {
        var beforeWorker = Host.Services.GetRequiredService<Worker>();
            
        foreach (var strategy in Enum.GetValues(typeof(Strategy)).OfType<Strategy>())
        {
            // The same class is doing all the work
            // its breaching the single responsibility principle, which is what the open closed principle helps solve
            // constantly modifying is going to add complexity to the individual class
            beforeWorker.DoWork(["item 1", "item 2", "item 3"], strategy);
        }
            
        var afterWorker = Host.Services.GetRequiredService<FastWorker>();
        afterWorker.DoWork(["item 1", "item 2", "item 3"]);
            
        var slowWorker = Host.Services.GetRequiredService<SlowWorker>();
        slowWorker.DoWork(["item 1", "item 2", "item 3"]);
        
        return Task.CompletedTask;
    }
}