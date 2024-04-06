using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brew.Features.Scripting.Pwsh;

class Program
{
    static Task Main()
    {
        return new Brew().RunAsync();
    }
}

public class Brew : ModuleBase
{
    protected override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        
    }
    
    protected override async Task ExecuteAsync(CancellationToken token = default)
    {
        var player = new Character("Hero", 100);
        var enemy = new Monster("Goblin", 50);

        using(var runspace = RunspaceFactory.CreateRunspace())
        {
            runspace.Open();
            runspace.SessionStateProxy.SetVariable("player", player);
            runspace.SessionStateProxy.SetVariable("enemy", enemy);
            runspace.SessionStateProxy.SetVariable("logger", new LoggerWrapper(Logger));
            
            using (var pwsh = PowerShell.Create(runspace))
            {
                pwsh.AddScript(Path.Combine(Directory.GetCurrentDirectory(), "Scripts", "combat.ps1"));
                await pwsh.InvokeAsync();
            
                if (pwsh.Streams.Error.Count > 0)
                {
                    Logger.LogError("Error in PowerShell script execution:");
                
                    foreach (var error in pwsh.Streams.Error)
                    {
                        Logger.LogError(error.Exception, "Error: {Error}", error.Exception.Message);
                    }
                }
            }    
        }
        
      
    }
}