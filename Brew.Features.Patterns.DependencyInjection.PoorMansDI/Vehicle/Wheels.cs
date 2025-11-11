using Microsoft.Extensions.Logging;

namespace Brew.Features.Patterns.DependencyInjection.PoorMansDI.Vehicle;

public interface IWheels
{
    string Traction();
}

public class Wheels : IWheels
{
    public Wheels()
    {
        ModuleBase.Logger.LogInformation("Wheels created: {Type}", GetType().FullName);
    }

    public virtual string Traction() => "Standard";
}