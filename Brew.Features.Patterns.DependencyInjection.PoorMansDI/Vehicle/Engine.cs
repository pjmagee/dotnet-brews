using Microsoft.Extensions.Logging;

namespace Brew.Features.Patterns.DependencyInjection.PoorMansDI.Vehicle;

public interface IEngine
{
    int HorsePower();
}

public class Engine : IEngine
{
    public Engine()
    {
        ModuleBase.Logger.LogInformation("Engine created: {Type}", GetType().FullName);
    }

    public virtual int HorsePower() => 150;
}