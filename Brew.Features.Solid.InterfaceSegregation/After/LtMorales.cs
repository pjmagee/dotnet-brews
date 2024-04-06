
using Microsoft.Extensions.Logging;

namespace Brew.Features.Solid.InterfaceSegregation.After;

public class LtMorales(ILogger<LtMorales> logger) : IHealer
{
    public void BasicAttack()
    {
        logger.LogInformation("This hero can basic attack");
    }

    public void Heal()
    {
        logger.LogInformation("This hero can heal");
    }
}