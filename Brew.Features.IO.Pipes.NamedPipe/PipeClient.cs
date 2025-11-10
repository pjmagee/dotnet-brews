using System.IO.Pipes;
using System.Text;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brew.Features.Pipes;

public class PipeClient(NamedPipeClientStream client, ILogger<PipeClient> logger) : BackgroundService
{
    public string ClientId => Guid.NewGuid().ToString();
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await Task.Delay(1000, stoppingToken); // Wait for server to start
            
            logger.LogInformation("{ClientId}: Client connecting to server...", ClientId);
            await client.ConnectAsync(stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Send "Hello" message
                    var message = Encoding.UTF8.GetBytes("Hello");
                    var messageLength = BitConverter.GetBytes(message.Length);
                    await client.WriteAsync(messageLength, stoppingToken);
                    await client.WriteAsync(message, stoppingToken);

                    // Read response length
                    var lengthBuffer = new byte[4];
                    await client.ReadExactlyAsync(lengthBuffer, stoppingToken);
                    int responseLength = BitConverter.ToInt32(lengthBuffer, 0);

                    var buffer = new byte[responseLength];
                    await client.ReadExactlyAsync(buffer, stoppingToken);
                    var response = Encoding.UTF8.GetString(buffer);
                    logger.LogInformation("{ClientId}: Client received: {Response}", ClientId, response);

                    await Task.Delay(1000, stoppingToken);
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
            
            logger.LogInformation("{ClientId}: PipeClient is shutting down", ClientId);
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("{ClientId}: PipeClient has been cancelled", ClientId);
        }
        finally
        {
            if (client.IsConnected)
            {
                client.Close();
            }
        }
    }
}