namespace Brew.Features.Solid.InterfaceSegregation.After;

/// <summary>
/// Segregated interface for ganker-specific behavior
/// Only heroes that can gank need to implement this
/// </summary>
public interface IGanker : IHeroAfter
{
    void Gank();
}