using Microsoft.Extensions.Logging;

namespace Brew.Features.Solid.InterfaceSegregation.After;

public class Genji(ILogger<Genji> logger) : IGanker, IAssassin
{
    public void BasicAttack()
    {
        logger.LogInformation("This hero can basic melee attack");
    }

    public void Assassinate()
    {
        logger.LogInformation("This hero can assassinate enemies");
    }

    public void Gank()
    {
        logger.LogInformation("This hero can gank enemies with his dive");
    }
}