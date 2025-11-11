using Microsoft.Extensions.Logging;

namespace Brew.Features.Patterns.DependencyInjection.PoorMansDI.Vehicle;

// Car now depends on abstractions (interfaces) instead of concrete implementations.
// This allows different wheel/engine/chassis combinations to be supplied externally
// (either manually or via a DI container) while the "poor man's" version will
// still new-up concrete types internally, demonstrating tight coupling.
public abstract class Car
{
    protected IWheels Wheels { get; private set; } = null!;
    protected IEngine Engine { get; private set; } = null!;
    protected IChassis Chassis { get; private set; } = null!;

    protected void SetComponents(IWheels wheels, IEngine engine, IChassis chassis)
    {
        Wheels = wheels;
        Engine = engine;
        Chassis = chassis;
    }

    public void Drive()
    {
        ModuleBase.Logger.LogInformation(
            "Driving -> HP={HP} Traction={Traction} ChassisMaterial={Material} ({Wheels}/{Engine}/{Chassis})",
            Engine.HorsePower(), Wheels.Traction(), Chassis.Material(),
            Wheels.GetType().Name, Engine.GetType().Name, Chassis.GetType().Name);
    }
}