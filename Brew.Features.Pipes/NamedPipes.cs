using System.IO.Pipes;
using System.Security.Principal;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Brew.Features.Pipes;

public class NamedPipes
{
    private readonly ILogger<NamedPipes> logger;

    public NamedPipes(ILogger<NamedPipes> logger)
    {
        this.logger = logger;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var serverTask = RunServer(cancellationToken);
        var clientTask = RunClient(cancellationToken);
            
        await Task.WhenAll(serverTask, clientTask);
    }

    private async Task RunClient(CancellationToken cancellationToken)
    {
        using (var pipeClient = new NamedPipeClientStream(".", "testpipe", PipeDirection.InOut, PipeOptions.None, TokenImpersonationLevel.Impersonation))
        {
            logger.LogInformation("Connecting to server...");
            await pipeClient.ConnectAsync(cancellationToken);

            var message = "HELLO";
            var bytes = Encoding.UTF8.GetBytes(message);

            while (!cancellationToken.IsCancellationRequested)
            {
                await pipeClient.WriteAsync(bytes, cancellationToken);
                await Task.Delay(500, cancellationToken);
            }
        }
    }

    private async Task RunServer(CancellationToken cancellationToken)
    {
        using (NamedPipeServerStream pipeServer = new NamedPipeServerStream("testpipe"))
        {
            logger.LogInformation("Waiting for connection...");
            await pipeServer.WaitForConnectionAsync(cancellationToken);
                
            using (StreamReader sw = new StreamReader(pipeServer, Encoding.UTF8))
            {
                while (!sw.EndOfStream)
                {
                    var result = await sw.ReadLineAsync(cancellationToken);
                    logger.LogInformation("server received: {Result}", result);
                }
            }
        }
    }
}