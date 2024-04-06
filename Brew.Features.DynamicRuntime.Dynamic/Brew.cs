using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Brew.Features.DynamicRuntime.Dynamic;

public class Brew : ModuleBase
{
    protected override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        services.AddSingleton<Anything>();
    }

    protected override Task ExecuteAsync(CancellationToken token = default)
    {
        Host.Services.GetRequiredService<Anything>().Execute();
        return Task.CompletedTask;
    }
}