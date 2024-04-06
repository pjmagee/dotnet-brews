using Microsoft.Extensions.Logging;

namespace Brew.Features.Patterns.DependencyInjection.PoorMansDI.Vehicle;

public abstract class Car
{
    protected Wheels Wheels;
    protected Engine Engine;
    protected Chassis Chassis;

    protected Car()
    {
        Wheels = new Wheels();
        Engine = new Engine();
        Chassis = new Chassis();
    }

    public void Drive()
    {
        ModuleBase.Logger.LogInformation("Car is driving");
    }
}