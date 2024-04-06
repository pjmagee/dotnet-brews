using System.IO.Pipes;
using System.Security.Principal;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Brew.Features.Pipes;

internal class NamedPipes(ILogger<NamedPipes> logger)
{
    private readonly string _pipeName = Guid.NewGuid().ToString();
    
    internal async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var server = Task.Run(() => RunServer(cancellationToken), cancellationToken);
        var client = Task.Run(() => RunClient(cancellationToken), cancellationToken);
        await Task.WhenAll(server, client);
    }

    private async Task RunClient(CancellationToken cancellationToken)
    {
        using (var pipeClient = new NamedPipeClientStream(
                   serverName: ".",
                   pipeName: _pipeName,
                   PipeDirection.InOut, 
                   PipeOptions.Asynchronous, 
                   TokenImpersonationLevel.Impersonation))
        {
            logger.LogInformation("Connecting to server...");
            await pipeClient.ConnectAsync(cancellationToken);
            logger.LogInformation("Connected to server...");
            
            var bytes = "HELLO"u8.ToArray();

            while (pipeClient.IsConnected && !cancellationToken.IsCancellationRequested)
            {
                logger.LogInformation("client sending: {Bytes}", bytes);
                
                
                await pipeClient.WriteAsync(bytes, cancellationToken);
                await Task.Delay(500, cancellationToken);
                
                if (pipeClient.CanRead)
                {
                    using (StreamReader sr = new StreamReader(pipeClient, Encoding.UTF8))
                    {
                        var result = await sr.ReadLineAsync(cancellationToken);
                        logger.LogInformation("client received: {Result}", result);
                    }
                }
            }
        }
    }

    private async Task RunServer(CancellationToken cancellationToken)
    {
        using (NamedPipeServerStream pipeServer = new NamedPipeServerStream(
                   pipeName: _pipeName, 
                   PipeDirection.InOut,
                   1, 
                   PipeTransmissionMode.Byte, 
                   PipeOptions.Asynchronous))
        {
            logger.LogInformation("Waiting for connection...");
            
            while (pipeServer.IsConnected && !cancellationToken.IsCancellationRequested)
            {
                using (StreamReader sw = new StreamReader(pipeServer, Encoding.UTF8))
                {
                    while (!sw.EndOfStream)
                    {
                        var result = await sw.ReadLineAsync(cancellationToken);
                        logger.LogInformation("server received: {Result}", result);
                    
                        if (result == "HELLO")
                        {
                            logger.LogInformation("server sending: {Result}", "WORLD");
                            await pipeServer.WriteAsync(Encoding.UTF8.GetBytes("WORLD"), cancellationToken);
                        }
                    }
                }
            }
        }
    }
}