using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brew.Features.PatternMatching;

public class Brew : IBrew
{
    public Task Before()
    {
        using (var host = Host.CreateDefaultBuilder().Build())
        {
            var logger = host.Services.GetRequiredService<ILogger<Brew>>();
            
            var hero = new Hero();
            var another = new Hero();

            int result;

            if (hero.Name == "Superman" && hero.Age > 3 && hero.Age < 10) result = 1;
            else if ("Batman".Equals(hero.Name) && hero.Age < 3) result = 2;
            else if (hero == another) result = 3;
            else result = 4;

            logger.LogInformation("pattern match result before: {Result}", + result);    
        }
        
        return Task.CompletedTask;
    }

    public Task After()
    {
        using (var host = Host.CreateDefaultBuilder().Build())
        {
            var logger = host.Services.GetRequiredService<ILogger<Brew>>();
            
            var hero = new Hero();
            var another = new Hero();

            int result = hero switch
            {
                { Name: "Superman", Age: > 3 and < 10 } => 1,
                { Name: "Batman", Age: < 3 } => 2,
                var o when o == another => 3,
                _ => 4
            };

            logger.LogInformation("pattern match result after: {Result}", + result);
        }
        
        return Task.CompletedTask;
    }
}