namespace Brew.Features.Patterns.Adaptor;

public class Adaptee
{
    public string Hello() => "World";
    public Entity GetEntity() => new Entity();
}