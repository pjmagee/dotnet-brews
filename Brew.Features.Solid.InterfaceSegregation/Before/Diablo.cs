using Microsoft.Extensions.Logging;

namespace Brew.Features.Solid.InterfaceSegregation.Before;

/// <summary>
/// Diablo is a Tank/Bruiser hero who can peel for teammates
/// PROBLEM: Forced to implement ALL methods from IHeroBefore even though:
/// - He cannot heal (he's not a healer)
/// - He cannot assassinate (he's not an assassin)
/// - He cannot gank effectively (he's a tank, not a ganker)
/// This violates ISP - classes shouldn't be forced to implement interfaces they don't use
/// </summary>
public class Diablo(ILogger<Diablo> logger) : IHeroBefore
{
    // ❌ Forced to implement - doesn't make sense for this hero
    public void Heal()
    {
        logger.LogError("This hero can't heal team members");
    }

    // ✓ Makes sense for this hero type
    public void Peel()
    {
        logger.LogInformation("This hero can peel enemies off team members");
    }

    // ❌ Forced to implement - doesn't make sense for this hero
    public void Assassinate()
    {
        logger.LogError("This hero can't assassinate enemies");
    }

    // ❌ Forced to implement - doesn't make sense for this hero
    public void Gank()
    {
        logger.LogError("This hero can't gank enemies with his dive");
    }

    // ✓ Makes sense for this hero type
    public void BasicAttack()
    {
        logger.LogInformation("This hero can basic melee attack");
    }
}