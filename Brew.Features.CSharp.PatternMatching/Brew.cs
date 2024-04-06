using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brew.Features.PatternMatching;

public class Brew : ModuleBase
{
    protected override Task BeforeAsync(CancellationToken token = default)
    {
        var hero = new Hero();
        var another = new Hero();

        int result;

        if (hero.Name == "Superman" && hero.Age > 3 && hero.Age < 10) result = 1;
        else if ("Batman".Equals(hero.Name) && hero.Age < 3) result = 2;
        else if (hero == another) result = 3;
        else result = 4;
        
        Logger.LogInformation("if else result before: {Result}", + result);
        
        return Task.CompletedTask;
    }

    protected override Task AfterAsync(CancellationToken token = default)
    {
        var hero = new Hero();
        var another = new Hero();

        int result = hero switch
        {
            { Name: "Superman", Age: > 3 and < 10 } => 1,
            { Name: "Batman", Age: < 3 } => 2,
            var o when o == another => 3,
            _ => 4
        };

        Logger.LogInformation("pattern match result after: {Result}", + result);
        
        return Task.CompletedTask;
    }

    protected override Task ExecuteAsync(CancellationToken token = default)
    {
        return Task.CompletedTask;
    }

    protected override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        
    }
}