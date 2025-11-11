using Brew.Features.Patterns.DependencyInjection.PoorMansDI.Vehicle;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brew.Features.Patterns.DependencyInjection.PoorMansDI;

/*
 * Dependency Injection is the concept of providing a dependency to a class via a property or constructor
 * Instead of the class itself instantiating what it requires, it is passed to it.
 *
 * This allows flexible behaviour when passing in abstract instances, i.e abstract class or interface
 */
public class Brew : ModuleBase
{
    protected override Task BeforeAsync(CancellationToken token = default)
    {
        ModuleBase.Logger.LogInformation("================ POOR MAN'S DI (Anti-Pattern) ================");
        ModuleBase.Logger.LogInformation("ConcreteCar directly instantiates ALL dependencies inside its constructor.");
        ModuleBase.Logger.LogInformation("Drawbacks: Tight coupling, difficult substitution, harder unit testing, violates SRP & OCP.");
        var car = new ConcreteCar();
        car.Drive();
        ModuleBase.Logger.LogInformation("--------------------------------------------------------------");
        return Task.CompletedTask;
    }

    protected override Task AfterAsync(CancellationToken token = default)
    {
        ModuleBase.Logger.LogInformation("================ CONTAINER-DRIVEN DI (Preferred) =============");
        ModuleBase.Logger.LogInformation("FlexibleCar receives abstractions via DI container.");
        ModuleBase.Logger.LogInformation("Benefits: Substitutability, testability (mock interfaces), configurability, adherence to OCP.");

        // Resolve from container (configured in ConfigureServices)
        var car = Host.Services.GetRequiredService<Car>();
        car.Drive();

        ModuleBase.Logger.LogInformation("=========================== SUMMARY ==========================");
        ModuleBase.Logger.LogInformation("Anti-Pattern: new ConcreteCar() => implicit hidden dependencies, change requires code modifications.");
        ModuleBase.Logger.LogInformation("DI Pattern: container resolves Car with IWheels/IEngine/IChassis => swap implementations without touching consumer.");
        ModuleBase.Logger.LogInformation("==============================================================");
        return Task.CompletedTask;
    }
    
    protected override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        // Register abstractions to concrete implementations. Swapping behavior is a one-line change.
        services.AddTransient<IWheels, ShinyWheels>();
        services.AddTransient<IEngine, FastEngine>();
        services.AddTransient<IChassis, SportsChassis>();
        services.AddTransient<Car, FlexibleCar>();
    }

    protected override Task ExecuteAsync(CancellationToken token = default)
    {
        return Task.CompletedTask;
    }
}