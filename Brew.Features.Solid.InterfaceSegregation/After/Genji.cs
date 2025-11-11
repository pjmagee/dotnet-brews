using Microsoft.Extensions.Logging;

namespace Brew.Features.Solid.InterfaceSegregation.After;

/// <summary>
/// Genji is an Assassin/Ganker hero
/// FOLLOWS ISP: Only implements interfaces for capabilities he actually has
/// - IAssassin: He can assassinate
/// - IGanker: He can gank with his mobility
/// - Does NOT implement IHealer or IPeeler (he can't do those)
/// </summary>
public class Genji(ILogger<Genji> logger) : IGanker, IAssassin
{
    public void BasicAttack()
    {
        logger.LogInformation("Genji can basic melee attack with shuriken");
    }

    public void Assassinate()
    {
        logger.LogInformation("Genji can assassinate enemies with swift strike");
    }

    public void Gank()
    {
        logger.LogInformation("Genji can gank enemies with his dive ability");
    }
}