using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brew.Features.Pipes;

public class Brew : IBrew
{
    public async Task Execute()
    {
        using (var host = Host
                   .CreateDefaultBuilder()
                   .ConfigureLogging(x => x.AddConsole())
                   .ConfigureServices(collection => collection.AddSingleton<NamedPipes>())
                   .Build())
        {
            var pipes = host.Services.GetRequiredService<NamedPipes>();
            await pipes.ExecuteAsync(CancellationToken.None); 
        }  
    }
}