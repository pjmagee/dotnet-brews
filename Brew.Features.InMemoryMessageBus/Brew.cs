using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Brew.Features.InMemoryMessageBus;

public class Brew : IBrew
{
    public async Task Execute()
    {
        var builder = Host.CreateApplicationBuilder();
        
        builder.Services
            .AddLogging()
            .AddSingleton<IEventBus, EventBus>()
            .AddSingleton<InMemoryMessageQueue>()
            .AddHostedService<QueueProcessor>()
            .AddHostedService<QueueWriter>();

        using (var host = builder.Build())
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(5));
            await Task.WhenAll(host.RunAsync(cts.Token));    
        }
    }
}