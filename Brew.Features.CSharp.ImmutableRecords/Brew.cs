using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brew.Features.ImmutableRecords;

public class Brew : ModuleBase
{
    protected override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        
    }

    protected override Task ExecuteAsync(CancellationToken token = default)
    {
        var person = new ReadOnlyPerson("Patrick", new DateTime(1990, 05, 08));
        Logger.LogInformation("Person: {Name} {DateOfBirth}", person.Name, person.DateOfBirth);

        var (name, dob) = new Person("Patrick", new DateTime(2021, 05, 08));
        Logger.LogInformation("Person: {Name} {DateOfBirth}", name, dob);
        
        return Task.CompletedTask;
    }
}