using Microsoft.Extensions.Logging;

namespace Brew.Features;

public class PatternMatching(ILogger<PatternMatching> logger) : IBrew
{
    public class Hero
    {
        public string? Name { get; set; }
        public int Age { get; set; }
    }

    public void Before()
    {
        var hero = new Hero();
        var another = new Hero();

        int result;

        if (hero.Name == "Superman" && hero.Age > 3 && hero.Age < 10) result = 1;
        else if ("Batman".Equals(hero.Name) && hero.Age < 3) result = 2;
        else if (hero == another) result = 3;
        else result = 4;

        logger.LogInformation("pattern match result before: {Result}", + result);
    }

    public void After()
    {
        var hero = new Hero();
        var another = new Hero();

        int result = hero switch
        {
            Hero { Name: "Superman", Age: > 3 and < 10 } => 1,
            Hero { Name: "Batman", Age: < 3 } => 2,
            var o when o == another => 3,
            _ => 4
        };

        logger.LogInformation("pattern match result after: {Result}", + result);
    }
}
