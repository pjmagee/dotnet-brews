using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Brew.Features.CSharp.HiddenKeywords;

public class Brew : ModuleBase
{
    protected override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        services.AddSingleton<HiddenRefKeywords>();
        services.AddSingleton<HiddenArgListKeyWord>();
    }

    protected override Task ExecuteAsync(CancellationToken token = default)
    {
        var hiddenRefKeywords = Host.Services.GetRequiredService<HiddenRefKeywords>();
        hiddenRefKeywords.Before();
        hiddenRefKeywords.After();
                
        var hiddenArgListKeyWord = Host.Services.GetRequiredService<HiddenArgListKeyWord>();
        hiddenArgListKeyWord.Before();
        hiddenArgListKeyWord.After();
        
        return Task.CompletedTask;
    }
}