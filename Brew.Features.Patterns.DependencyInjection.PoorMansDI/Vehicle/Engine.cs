using Microsoft.Extensions.Logging;

namespace Brew.Features.Patterns.DependencyInjection.PoorMansDI.Vehicle;

public class Engine
{
    public Engine()
    {
        ModuleBase.Logger.LogInformation("Engine created: {Type}", GetType().FullName);
    }
}