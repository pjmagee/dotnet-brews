using System;
using System.Dynamic;
using Microsoft.Extensions.Logging;

namespace Brew.Features;

public class DynamicRuntime(ILogger<DynamicRuntime> logger) : IBrew
{
    public void Execute()
    {
        dynamic anything = new ExpandoObject();

        anything.Name = "Duck";
        anything.Quack = new Action(() => logger.LogInformation("Quack quack"));
        anything.Quack();

        anything = new ExpandoObject();
        anything.Name = "Dog";
        anything.Woof = new Action(() => logger.LogInformation("Woof woof"));
        anything.Woof();

        anything.Number = int.MaxValue;
        logger.LogInformation($"{anything.Number}");
        anything.Number = decimal.MaxValue;
        logger.LogInformation($"{anything.Number}");
        anything.Number = double.MaxValue;
        logger.LogInformation($"{anything.Number}");

        anything.AskPermission = new Func<string, bool>((message) => true);

        dynamic result = anything.AskPermission("Do i have permission?");

        if (result)
        {
            logger.LogInformation("Permission granted");
        }
    }
}
