
namespace Brew.Features.Builders.Simple;

public class EntityBuilder
{
    private Entity entity = new();

    public Entity Build() => entity;

    public EntityBuilder WithProperty1(bool value)
    {
        entity.Property1 = value;
        return this;
    }

    public EntityBuilder WithNumberAdded(int number)
    {
        entity.Numbers.Add(number);
        return this;
    }

    public EntityBuilder WithProperty2(string value)
    {
        entity.Property2 = value;
        return this;
    }

    public EntityBuilder WithProperty3(params KeyValuePair<string, string>[] values)
    {
        entity.Property3 = new Dictionary<string, string>(values);
        return this;
    }
}