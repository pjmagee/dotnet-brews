using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Brew.Features.Patterns.Singleton;

public class Brew : ModuleBase
{
    protected override Task BeforeAsync(CancellationToken token = default)
    {
        Person person1 = new Person();
        person1.Speak();

        person1 = new Person();
        person1.Speak();
        
        return Task.CompletedTask;
    }

    protected override Task AfterAsync(CancellationToken token = default)
    {
        SingletonPerson person = SingletonPerson.Me;
        person.Speak();

        // Grab same single instance
        person = SingletonPerson.Me; 

        // will be same age as first one
        person.Speak(); 
        
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