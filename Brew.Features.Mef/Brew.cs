using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brew.Features.Mef;

public class Brew : ModuleBase
{
    protected override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        services.AddSingleton<CalculatorOperationsLoader>(x =>
        {
            var logger = x.GetRequiredService<ILogger<CalculatorOperationsLoader>>();
            var pluginDir = Directory.GetCurrentDirectory();
            return new CalculatorOperationsLoader(logger, pluginDir);
        });
                
        services.AddSingleton<Calculator>();
    }

    protected override Task ExecuteAsync(CancellationToken token = default)
    {
        var calculator = Host.Services.GetRequiredService<Calculator>();
            
        foreach(var op in calculator.Operatons)
        {
            calculator.Calculate(op, 10, 5);
        }
        
        return Task.CompletedTask;
    }
}