using System.Dynamic;
using Microsoft.Extensions.Logging;

namespace Brew.Features.DynamicRuntime;

public class Anything(ILogger<Anything> logger)
{
    private dynamic anything = new ExpandoObject();
    
    public void Execute()
    {
        anything.Name = "Duck";
        anything.Quack = new Action(() => logger.LogInformation("Quack quack"));
        anything.Quack();

        anything = new ExpandoObject();
        anything.Name = "Dog";
        anything.Woof = new Action(() => logger.LogInformation("Woof woof"));
        anything.Woof();

        anything.Number = int.MaxValue;
        logger.LogInformation("{Number}", (string)anything.Number.ToString());
        anything.Number = decimal.MaxValue;
        logger.LogInformation("{Number}", (string)anything.Number.ToString());
        anything.Number = double.MaxValue;
        logger.LogInformation("{Number}", (string)anything.Number.ToString());

        anything.AskPermission = new Func<string, bool>((message) => true);

        dynamic result = anything.AskPermission("Do i have permission?");

        if (result)
        {
            logger.LogInformation("Permission granted");
        }
    }
    
}