using Microsoft.Extensions.Logging;

namespace Brew.Features.Solid.InterfaceSegregation.After;

/// <summary>
/// Diablo is a Tank/Peeler hero
/// FOLLOWS ISP: Only implements interfaces for capabilities he actually has
/// - IPeeler: He can peel enemies off teammates
/// - Does NOT implement IHealer, IAssassin, or IGanker (he can't do those effectively)
/// No need to implement methods that don't make sense for this hero!
/// </summary>
public class DiabloImproved(ILogger<DiabloImproved> logger) : IPeeler
{
    public void BasicAttack()
    {
        logger.LogInformation("Diablo can basic melee attack");
    }

    public void Peel()
    {
        logger.LogInformation("Diablo can peel enemies off team members with charge and flip");
    }
}
