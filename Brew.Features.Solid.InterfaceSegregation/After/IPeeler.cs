namespace Brew.Features.Solid.InterfaceSegregation.After;

/// <summary>
/// Segregated interface for peeler/tank-specific behavior
/// Only heroes that can peel enemies off teammates need to implement this
/// </summary>
public interface IPeeler : IHeroAfter
{
    void Peel();
}
