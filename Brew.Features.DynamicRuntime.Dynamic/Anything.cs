using System.Dynamic;
using Microsoft.Extensions.Logging;

namespace Brew.Features.DynamicRuntime.Dynamic;

public class Anything(ILogger<Anything> logger)
{
    private dynamic _anything = new ExpandoObject();
    
    public void Execute()
    {
        _anything.Name = "Duck";
        _anything.Quack = new Action(() => logger.LogInformation("Quack quack"));
        _anything.Quack();

        _anything = new ExpandoObject();
        _anything.Name = "Dog";
        _anything.Woof = new Action(() => logger.LogInformation("Woof woof"));
        _anything.Woof();

        _anything.Number = int.MaxValue;
        logger.LogInformation("{Number}", (string)_anything.Number.ToString());
        _anything.Number = decimal.MaxValue;
        logger.LogInformation("{Number}", (string)_anything.Number.ToString());
        _anything.Number = double.MaxValue;
        logger.LogInformation("{Number}", (string)_anything.Number.ToString());

        _anything.AskPermission = 1;
        _anything.AskPermission = new Func<string, bool>((message) => Random.Shared.Next(0, 2) == 1);

        dynamic result = _anything.AskPermission("Do i have permission?");
        logger.LogInformation("Permission {Status}", result ? "granted" : "denied");
    }
    
}