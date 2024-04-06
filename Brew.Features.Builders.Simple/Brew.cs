using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Brew.Features.Builders.Simple;

public class Brew : ModuleBase
{
    protected override Task AfterAsync(CancellationToken token = default)
    {
        EntityBuilder builder = new();

        Entity entity = builder
            .WithProperty1(true)
            .WithProperty2("property 2")
            .WithProperty3(new KeyValuePair<string, string>("A", "1"), new KeyValuePair<string, string>("B", "2"))
            .WithNumberAdded(1)
            .WithNumberAdded(5)
            .Build();
        
        return Task.CompletedTask;
    }

    protected override Task BeforeAsync(CancellationToken token = default)
    {
        Entity entity = new Entity();
        entity.Property1 = true;
        entity.Property2 = "";
        entity.Property3 = new Dictionary<string, string>();
        entity.Numbers.AddRange(new[] { 1, 5 });
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