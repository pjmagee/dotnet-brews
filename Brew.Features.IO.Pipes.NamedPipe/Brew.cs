using System.IO.Pipes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Brew.Features.Pipes;

public class Brew : ModuleBase
{
    protected override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        services
            .AddSingleton<NamedPipeClientStream>(x => new NamedPipeClientStream(".", "testpipe", PipeDirection.InOut, PipeOptions.Asynchronous))
            .AddSingleton<NamedPipeServerStream>(x => new NamedPipeServerStream("testpipe", PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous))
            .AddHostedService<PipeClient>()
            .AddHostedService<PipeServer>();
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