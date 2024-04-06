using Microsoft.Extensions.Logging;

namespace Brew.Features.Patterns.Strategy;

public class ConcreteStrategyB(ILogger<ConcreteStrategyB> logger) : IStrategy
{
    public void Execute()
    {
        logger.LogWarning("ConcreteStrategyB Execute");
    }
}