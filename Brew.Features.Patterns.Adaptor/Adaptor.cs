namespace Brew.Features.Patterns.Adaptor;

public class Adaptor(Adaptee adaptee)
{
    // Modify or forward to original function 
    public string Hello() => "Hello " + adaptee.Hello();

    // Can also Modify or wrap in custom class.
    public Entity GetEntity() => adaptee.GetEntity();
}