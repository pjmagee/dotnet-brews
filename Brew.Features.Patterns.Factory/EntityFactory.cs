namespace Brew.Features.Patterns.Factory;

public class EntityFactory
{
    public Entity Create(int type)
    {
        return type switch
        {
            1 => new Entity1(), // type 1 based on argument
            2 => new Entity2(), // type 2 based on argument
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}