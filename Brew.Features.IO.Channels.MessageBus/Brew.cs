using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brew.Features.IO.Channels.MessageBus;

/// <summary>
/// Demonstrates System.Threading.Channels for building an in-process message bus.
/// Uses async producer/consumer pattern with QueueWriter publishing events and QueueProcessor consuming them.
/// </summary>
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
        Logger.LogInformation("========== CHANNELS MESSAGE BUS DEMONSTRATION ==========");
        Logger.LogInformation("Scenario: In-memory event bus using System.Threading.Channels\n");
        Logger.LogInformation("QueueWriter publishes events every 1 second");
        Logger.LogInformation("QueueProcessor consumes events asynchronously\n");

        Logger.LogInformation("---------- BENEFITS OF CHANNELS ----------");
        Logger.LogInformation("✓ High-performance bounded/unbounded queues");
        Logger.LogInformation("✓ Built-in backpressure handling");
        Logger.LogInformation("✓ Lock-free concurrent access (single writer/reader)");
        Logger.LogInformation("✓ Seamless async/await integration");
        Logger.LogInformation("✓ Better alternative to BlockingCollection<T>");

        return Task.CompletedTask;
    }

    public override Task RunAsync(CancellationToken token = default)
    {
        CancellationTokenSource cts = new();
        cts.CancelAfter(TimeSpan.FromSeconds(5));
        return Host.RunAsync(cts.Token);
    }
}
