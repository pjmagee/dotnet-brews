using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brew.Features.HiddenKeywords;

public class Brew : IBrew
{
    public Task Execute()
    {
        using(var host = Host.CreateDefaultBuilder().ConfigureLogging(x => x.AddConsole()).ConfigureServices(collection => collection.AddSingleton<HiddenRefKeywords>().AddSingleton<HiddenArgListKeyWord>()).Build())
        {
            using(var scope = host.Services.CreateScope())
            {
                var hiddenRefKeywords = scope.ServiceProvider.GetRequiredService<HiddenRefKeywords>();
                hiddenRefKeywords.Before();
                hiddenRefKeywords.After();
                
                var hiddenArgListKeyWord = scope.ServiceProvider.GetRequiredService<HiddenArgListKeyWord>();
                hiddenArgListKeyWord.Before();
                hiddenArgListKeyWord.After();
            }
        }
        
        return Task.CompletedTask;
    }
}