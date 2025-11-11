namespace Brew.Features.Solid.InterfaceSegregation.After;

/// <summary>
/// Segregated interface for assassin-specific behavior
/// Only heroes that can assassinate need to implement this
/// </summary>
public interface IAssassin : IHeroAfter
{
    void Assassinate();
}