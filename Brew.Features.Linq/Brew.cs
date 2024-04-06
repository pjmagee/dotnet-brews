using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Brew.Features.Linq;

public class Brew : ModuleBase
{

    protected override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        
    }

    protected override Task ExecuteAsync(CancellationToken token = default)
    {
        List<Entity> entities = Enumerable
            .Range(0, 100) // The range (for i = 0; i < 100; i++ 
            .Select(i => new Entity() { Id = i }) // Select is like the body in our original for loop 
            .Where(x => x.Id >= 50) // this is like our if statement when adding to our filtered list
            .ToList(); // turn this into concrete List<int>
        
        return Task.CompletedTask;
    }
}