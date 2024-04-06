using Microsoft.Extensions.Logging;

namespace Brew.Features.Solid.InterfaceSegregation.Before;

public class Diablo(ILogger<Diablo> logger) : IHeroBefore
{
    public void Heal()
    {
        logger.LogError("This hero can't heal team members");
    }

    public void Peel()
    {
        logger.LogInformation("This hero can peel enemies off team members");
    }

    public void Assassinate()
    {
        logger.LogError("This hero can't assassinate enemies");
    }

    public void Gank()
    {
        logger.LogError("This hero can't gank enemies with his dive");
    }

    public void BasicAttack()
    {
        logger.LogInformation("This hero can basic melee attack");
    }
}