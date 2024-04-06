using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Brew.Features.CSharp.Events;

public class Brew : ModuleBase
{
    protected override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        services.AddSingleton<NativeEventNotifier>().AddSingleton<NativeEventReceiver>();
    }

    protected override Task ExecuteAsync(CancellationToken token = default)
    {
        Host.Services.GetRequiredService<NativeEventReceiver>().Subscribe();
        Host.Services.GetRequiredService<NativeEventNotifier>().Trigger();
        
        return Task.CompletedTask;
    }
}