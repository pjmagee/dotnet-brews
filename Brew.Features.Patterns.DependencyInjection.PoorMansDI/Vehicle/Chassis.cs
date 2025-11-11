namespace Brew.Features.Patterns.DependencyInjection.PoorMansDI.Vehicle;

public interface IChassis
{
    string Material();
}

public class Chassis : IChassis
{
    public Chassis()
    {
        Console.WriteLine(GetType());
    }

    public virtual string Material() => "Steel";
}