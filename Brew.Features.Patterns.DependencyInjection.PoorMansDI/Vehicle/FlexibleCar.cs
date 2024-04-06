namespace Brew.Features.Patterns.DependencyInjection.PoorMansDI.Vehicle;

public sealed class FlexibleCar : Car
{
    public FlexibleCar(Wheels wheels, Engine engine, Chassis chassis)
    {
        this.Wheels = wheels;
        this.Engine = engine;
        this.Chassis = chassis;
    }
}