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
        await Task.Delay(1000, stoppingToken); // Wait for server to start
        
        logger.LogInformation("{ClientId}: Client connecting to server...", ClientId);
        await client.ConnectAsync(stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            // Send "Hello" message
            var message = Encoding.UTF8.GetBytes("Hello");
            var messageLength = BitConverter.GetBytes(message.Length);
            await client.WriteAsync(messageLength, 0, messageLength.Length, stoppingToken);
            await client.WriteAsync(message, 0, message.Length, stoppingToken);

            // Read response length
            var lengthBuffer = new byte[4];
            await client.ReadAsync(lengthBuffer, 0, lengthBuffer.Length, stoppingToken);
            int responseLength = BitConverter.ToInt32(lengthBuffer, 0);

            
            var buffer = new byte[responseLength];
            await client.ReadAsync(buffer, 0, buffer.Length, stoppingToken);
            var response = Encoding.UTF8.GetString(buffer);
            logger.LogInformation("{ClientId}: Client received: {Response}", ClientId, response);

            
            await Task.Delay(1000, stoppingToken);
        }

        client.Close();
    }
}