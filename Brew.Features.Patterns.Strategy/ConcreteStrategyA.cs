using Microsoft.Extensions.Logging;

namespace Brew.Features.Patterns.Strategy;

public class ConcreteStrategyA(ILogger<ConcreteStrategyA> logger) : IStrategy
{
    public void Execute()
    {
        logger.LogInformation("ConcreteStrategyA Execute");
    }
}