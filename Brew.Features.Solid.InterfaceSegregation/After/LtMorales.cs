using Microsoft.Extensions.Logging;

namespace Brew.Features.Solid.InterfaceSegregation.After;

/// <summary>
/// Lt. Morales is a Support/Healer hero
/// FOLLOWS ISP: Only implements interfaces for capabilities she actually has
/// - IHealer: She can heal teammates
/// - Does NOT implement IAssassin, IGanker, or IPeeler (she can't do those)
/// </summary>
public class LtMorales(ILogger<LtMorales> logger) : IHealer
{
    public void BasicAttack()
    {
        logger.LogInformation("Lt. Morales can basic ranged attack");
    }

    public void Heal()
    {
        logger.LogInformation("Lt. Morales can heal teammates with healing beam");
    }
}