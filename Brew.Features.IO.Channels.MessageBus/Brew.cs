using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Brew.Features.IO.Channels.MessageBus;

public class Brew : ModuleBase
{
    protected override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        services
            .AddLogging()
            .AddSingleton<IEventBus, EventBus>()
            .AddSingleton<InMemoryMessageQueue>()
            .AddHostedService<QueueProcessor>()
            .AddHostedService<QueueWriter>();
    }

    protected override Task ExecuteAsync(CancellationToken token = default)
    {
        return Task.CompletedTask;
    }

    public override Task RunAsync(CancellationToken token = default)
    {
        CancellationTokenSource cts = new();
        cts.CancelAfter(TimeSpan.FromSeconds(5));
        return Host.RunAsync(cts.Token);
    }
}