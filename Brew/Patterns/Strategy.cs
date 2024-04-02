using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Brew.Patterns;

public class Strategy : IBrew
{
    public void Before()
    {
        var services = new ServiceCollection();
        services.AddScoped<Context>();
        services.AddScoped<IStrategy, ConcreteStrategyA>();
        services.AddLogging();

        using (var scope = services.BuildServiceProvider().CreateScope())
        {
            scope.ServiceProvider.GetRequiredService<Context>().Execute();
        }
    }

    public void After()
    {
        using (var scope = new ServiceCollection()
                   .AddKeyedScoped<Context>("ConcreteStrategyA")
                   .AddLogging()
                   .AddScoped<IStrategy, ConcreteStrategyA>()
                   .BuildServiceProvider())
        {
            scope.GetRequiredKeyedService<Context>("ConcreteStrategyA").Execute();
        }
        
        using (var scope = new ServiceCollection()
                   .AddKeyedScoped<Context>("ConcreteStrategyB")
                   .AddLogging()
                   .AddScoped<IStrategy, ConcreteStrategyB>()
                   .BuildServiceProvider())
        {
            scope.GetRequiredKeyedService<Context>("ConcreteStrategyB").Execute();
        }
    }

    public interface IStrategy
    {
        void Execute();
    }

    public class ConcreteStrategyA(ILogger<ConcreteStrategyA> logger) : IStrategy
    {
        public void Execute()
        {
            logger.LogInformation("ConcreteStrategyA Execute");
        }
    }

    public class ConcreteStrategyB(ILogger<ConcreteStrategyB> logger) : IStrategy
    {
        public void Execute()
        {
            logger.LogWarning("ConcreteStrategyB Execute");
        }
    }

    public class Context(IStrategy strategy)
    {
        public void Execute()
        {
            strategy.Execute();
        }
    }
}