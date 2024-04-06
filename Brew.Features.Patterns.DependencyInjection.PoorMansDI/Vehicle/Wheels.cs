using Microsoft.Extensions.Logging;

namespace Brew.Features.Patterns.DependencyInjection.PoorMansDI.Vehicle;

public class Wheels
{
    public Wheels()
    {
        ModuleBase.Logger.LogInformation("Wheels created: {Type}", GetType().FullName);
    }
}