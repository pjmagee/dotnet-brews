namespace Brew.Features.Patterns.DependencyInjection.PoorMansDI.Vehicle;

// Anti-pattern: this concrete car hard-codes specific implementations.
// Changing behaviour requires modifying this class (open/closed violation) and
// unit testing is harder because dependencies can't be substituted easily.
public sealed class ConcreteCar : Car
{
    public ConcreteCar()
    {
        SetComponents(new Wheels(), new Engine(), new Chassis());
    }
}