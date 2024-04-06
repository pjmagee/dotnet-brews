namespace Brew.Features.Patterns.DependencyInjection.PoorMansDI.Vehicle;

public sealed class ConcreteCar : Car
{
    public ConcreteCar()
    {
        Chassis = new Chassis();
        Engine = new Engine();
        Wheels = new Wheels();
        Wheels = new Wheels();
    }
}