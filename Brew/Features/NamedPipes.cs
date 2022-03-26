using System;
using System.IO;
using System.IO.Pipes;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Brew.Features;

public class NamedPipes : IBrew
{
    private readonly ILogger<NamedPipes> logger;

    public NamedPipes(ILogger<NamedPipes> logger)
    {
        this.logger = logger;
    }

    public void Execute()
    {
        CancellationTokenSource cancellationTokenSource = new();
        cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(2));

        try
        {
            Task.WaitAll(
                RunServer(cancellationTokenSource.Token),
                RunClient(cancellationTokenSource.Token));
        }
        catch
        {

        }
    }

    public async Task RunClient(CancellationToken cancellationToken)
    {
        using (var pipeClient = new NamedPipeClientStream(".", "testpipe", PipeDirection.InOut, PipeOptions.None, TokenImpersonationLevel.Impersonation))
        {
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

    public async Task RunServer(CancellationToken cancellationToken)
    {
        using (NamedPipeServerStream pipeServer = new NamedPipeServerStream("testpipe"))
        {
            await pipeServer.WaitForConnectionAsync(cancellationToken);
            
            using (StreamReader sw = new StreamReader(pipeServer, Encoding.UTF8))
            {
                while (!sw.EndOfStream)
                {
                    var result = await sw.ReadLineAsync();
                    logger.LogInformation("server received: " + result);
                }
            }
        }
    }
}