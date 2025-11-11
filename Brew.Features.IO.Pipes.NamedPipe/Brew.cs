using System.IO.Pipes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brew.Features.Pipes;

/// <summary>
/// Demonstrates Named Pipes for inter-process communication (IPC) on Windows/Linux.
/// Server waits for client connection, then exchanges messages bi-directionally.
/// </summary>
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
        Logger.LogInformation("========== NAMED PIPES IPC DEMONSTRATION ==========");
        Logger.LogInformation("Scenario: Bi-directional communication between client/server processes\n");
        Logger.LogInformation("PipeServer waits for connection, then receives/responds to messages");
        Logger.LogInformation("PipeClient connects to server and exchanges messages\n");

        Logger.LogInformation("---------- BENEFITS OF NAMED PIPES ----------");
        Logger.LogInformation("✓ Fast local IPC (same machine processes)");
        Logger.LogInformation("✓ Secure (OS-level access control)");
        Logger.LogInformation("✓ Cross-platform (Windows/Linux/macOS)");
        Logger.LogInformation("✓ Supports impersonation (Windows security context)");
        Logger.LogInformation("✓ Ideal for microservices on same host");

        return Task.CompletedTask;
    }

    public override Task RunAsync(CancellationToken token = default)
    {
        CancellationTokenSource cts = new();
        cts.CancelAfter(TimeSpan.FromSeconds(5));
        return Host.RunAsync(cts.Token);
    }
}

