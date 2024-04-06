using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Brew.Features.Patterns.Strategy;

public class Brew : ModuleBase
{
    protected override Task BeforeAsync(CancellationToken token = default)
    {
        var services = new ServiceCollection();
        services.AddScoped<Context>();
        services.AddScoped<IStrategy, ConcreteStrategyA>();
        services.AddLogging();

        services.BuildServiceProvider().GetRequiredService<Context>().Execute();
        
        return Task.CompletedTask;
    }

    protected override Task AfterAsync(CancellationToken token = default)
    {
        var services = new ServiceCollection()
            .AddKeyedScoped<Context>("ConcreteStrategyA")
            .AddKeyedScoped<Context>("ConcreteStrategyB")
            .AddLogging()
            .AddScoped<IStrategy, ConcreteStrategyA>()
            .AddScoped<IStrategy, ConcreteStrategyB>()
            .BuildServiceProvider();
        
        // Execute ConcreteStrategyA
        services.GetRequiredKeyedService<Context>("ConcreteStrategyA").Execute();
        
        // Execute ConcreteStrategyB
        services.GetRequiredKeyedService<Context>("ConcreteStrategyB").Execute();
        
        return Task.CompletedTask;
    }


    protected override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        
    }

    protected override Task ExecuteAsync(CancellationToken token = default)
    {
        return Task.CompletedTask;
    }
}