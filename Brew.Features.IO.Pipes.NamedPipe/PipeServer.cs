using System.IO.Pipes;
using System.Text;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brew.Features.Pipes;

public class PipeServer(NamedPipeServerStream server, ILogger<PipeServer> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Server waiting for connection...");
        await server.WaitForConnectionAsync(stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            // Read message length
            var lengthBuffer = new byte[4];
            await server.ReadAsync(lengthBuffer, 0, lengthBuffer.Length, stoppingToken);
            int messageLength = BitConverter.ToInt32(lengthBuffer, 0);

            // Read message based on length
            var buffer = new byte[messageLength];
            await server.ReadAsync(buffer, 0, buffer.Length, stoppingToken);
            var message = Encoding.UTF8.GetString(buffer);
            logger.LogInformation("Server received: {Message}", message);
            
            // Respond back
            var response = Encoding.UTF8.GetBytes("World");
            var responseLength = BitConverter.GetBytes(response.Length);
            await server.WriteAsync(responseLength, 0, responseLength.Length, stoppingToken);
            await server.WriteAsync(response, 0, response.Length, stoppingToken);
            logger.LogInformation("Server sent: {Response} to Client {Client}", "World", server.GetImpersonationUserName());
        }

        server.Close();
    }
}