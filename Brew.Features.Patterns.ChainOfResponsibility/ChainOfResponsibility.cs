using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Brew.Features.Patterns.ChainOfResponsibility;


public class ChainOfResponsibility : ModuleBase
{
    protected override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        
    }

    protected override Task ExecuteAsync(CancellationToken token = default)
    {
        var ceo = new Ceo();
        var midLevel = new MidLevel();
        var entryLevel = new EntryLevel();

        midLevel.SetSuccessor(ceo);
        entryLevel.SetSuccessor(midLevel);

        entryLevel.SignOff(new Request() { Type = RequestType.Level1 });
        entryLevel.SignOff(new Request() { Type = RequestType.Level2 });
        entryLevel.SignOff(new Request() { Type = RequestType.Level3 });
        
        return Task.CompletedTask;
    }
}