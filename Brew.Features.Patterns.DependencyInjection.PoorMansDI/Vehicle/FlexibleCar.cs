namespace Brew.Features.Patterns.DependencyInjection.PoorMansDI.Vehicle;

// Flexible car receives abstractions (interfaces) enabling substitution (e.g. test doubles).
public sealed class FlexibleCar : Car
{
    public FlexibleCar(IWheels wheels, IEngine engine, IChassis chassis)
    {
        SetComponents(wheels, engine, chassis);
    }
}