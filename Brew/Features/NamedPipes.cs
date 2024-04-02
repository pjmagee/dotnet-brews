using System;
using System.IO;
using System.IO.Pipes;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Brew.Features;

public class NamedPipes(ILogger<NamedPipes> logger) : IBrew
{
    public void Execute()
    {
        using (CancellationTokenSource cancellationTokenSource = new())
        {
            cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(5));

            try
            {
                Task.WaitAll([
                        RunServer(cancellationTokenSource.Token),
                        RunClient(cancellationTokenSource.Token)
                    ],
                    cancellationTokenSource.Token);
            }
            catch (OperationCanceledException)
            {
                
            }
            catch (AggregateException)
            {
                
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error occurred");
            }
            finally
            {
            
            }
        }
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