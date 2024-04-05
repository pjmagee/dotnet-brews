using Brew.Feature.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brew.Features.Events;

public class Brew : IBrew
{
    public Task Execute()
    {
        using (var host = Host.CreateDefaultBuilder()
                   .ConfigureLogging(x => x.AddConsole())
                   .ConfigureServices(collection =>
                       collection.AddSingleton<NativeEventNotifier>().AddSingleton<NativeEventReciever>())
                   .Build())
        {
            using(var scope = host.Services.CreateScope())
            {
                var notifier = scope.ServiceProvider.GetRequiredService<NativeEventNotifier>();
                var reciever = scope.ServiceProvider.GetRequiredService<NativeEventReciever>();
            
                reciever.Subscribe();
                notifier.Trigger();
            }    
        }
        
        return Task.CompletedTask;
    }
}