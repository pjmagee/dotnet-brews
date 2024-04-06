namespace Brew.Features.Patterns.Strategy;

public class Context(IStrategy strategy)
{
    public void Execute()
    {
        strategy.Execute();
    }
}