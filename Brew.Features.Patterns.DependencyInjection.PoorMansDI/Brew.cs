using Brew.Features.Patterns.DependencyInjection.PoorMansDI.Vehicle;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
        // Anti-Pattern because it is hard to test and maintain as the class is responsible for creating its own dependencies
        ConcreteCar car = new ConcreteCar();
        car.Drive();
        
        return Task.CompletedTask;
    }

    protected override Task AfterAsync(CancellationToken token = default)
    {
        // Dependency Injection allows for the class to be passed its dependencies
        // This is a better pattern as it allows for easier testing and maintenance
        FlexibleCar car = new FlexibleCar(new ShinyWheels(), new FastEngine(), new SportsChassis());
        
        car.Drive();
        
        return Task.CompletedTask;
    }
    
    protected override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        
    }

    protected override Task ExecuteAsync(CancellationToken token = default)
    {
        return Task.CompletedTask;
    }
}