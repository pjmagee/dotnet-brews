namespace Brew.Features.Solid.InterfaceSegregation.After;

/// <summary>
/// Segregated interface for healer-specific behavior
/// Only heroes that can heal need to implement this
/// </summary>
public interface IHealer : IHeroAfter
{
    void Heal();
}