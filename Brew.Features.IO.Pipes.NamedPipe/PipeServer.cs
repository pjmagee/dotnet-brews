using System.IO.Pipes;
using System.Text;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brew.Features.Pipes;

public class PipeServer(NamedPipeServerStream server, ILogger<PipeServer> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            Console.WriteLine("Server waiting for connection...");
            await server.WaitForConnectionAsync(stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Read message length
                    var lengthBuffer = new byte[4];
                    await server.ReadExactlyAsync(lengthBuffer, stoppingToken);
                    int messageLength = BitConverter.ToInt32(lengthBuffer, 0);

                    // Read message based on length
                    var buffer = new byte[messageLength];
                    await server.ReadExactlyAsync(buffer, stoppingToken);
                    var message = Encoding.UTF8.GetString(buffer);
                    logger.LogInformation("Server received: {Message}", message);
                    
                    // Respond back
                    var response = Encoding.UTF8.GetBytes("World");
                    var responseLength = BitConverter.GetBytes(response.Length);
                    await server.WriteAsync(responseLength, stoppingToken);
                    await server.WriteAsync(response, stoppingToken);
                    logger.LogInformation("Server sent: {Response} to Client {Client}", "World", server.GetImpersonationUserName());
                }
                catch (OperationCanceledException)
                {
                    // Exit the loop on cancellation
                    break;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error processing pipe message");
                }
            }
            
            logger.LogInformation("PipeServer is shutting down");
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("PipeServer has been cancelled");
        }
        finally
        {
            if (server.IsConnected)
            {
                server.Disconnect();
            }
            server.Close();
        }
    }
}