namespace Brew.Features.Patterns.Factory;

public class BeforeConsumer
{
    private Entity _entity = new();

    // New entity within the consumer

    public void DoSomething() => Console.WriteLine(_entity.ToString());
}