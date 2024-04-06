using IronPython.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Scripting.Hosting;

namespace Brew.Features.Scripting.IPython;

public class Brew : ModuleBase
{
    protected override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        services.AddSingleton<ScriptEngine>(x => Python.CreateEngine());
    }

    protected override Task ExecuteAsync(CancellationToken token = default)
    {
        var character = new Character("Hero", 100);
        var monster = new Monster("Goblin", 50);

        var engine = Host.Services.GetRequiredService<ScriptEngine>();

        var scope = engine.CreateScope();
        scope.SetVariable("character", character);
        scope.SetVariable("monster", monster);
        scope.SetVariable("logger", new LoggerWrapper(Logger));
        
        engine.ExecuteFile(Path.Combine(Directory.GetCurrentDirectory(), "Scripts", "combat.py"), scope);

        return Task.CompletedTask;
    }
}