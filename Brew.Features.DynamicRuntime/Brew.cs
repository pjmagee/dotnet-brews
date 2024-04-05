using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brew.Features.DynamicRuntime;

public class Brew : IBrew
{
    public Task Execute()
    {
        using (var host = Host.CreateDefaultBuilder()
                   .ConfigureServices(x => x.AddSingleton<Anything>())
                   .ConfigureLogging(x => x.AddConsole())
                   .Build())
        {
            host.Services.GetRequiredService<Anything>().Execute();
        }
        
        return Task.CompletedTask;
    }
}