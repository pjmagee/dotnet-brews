namespace Brew.Features.Patterns.Factory;

public class AfterConsumer(EntityFactory factory)
{
    private Entity _entity = factory.Create(1);

    public void DoSomething() => Console.WriteLine(_entity.ToString());
}