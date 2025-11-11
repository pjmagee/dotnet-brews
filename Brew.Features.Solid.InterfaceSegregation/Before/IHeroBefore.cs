namespace Brew.Features.Solid.InterfaceSegregation.Before;

/// <summary>
/// VIOLATES Interface Segregation Principle:
/// - "Fat" interface with too many methods
/// - Forces implementations to implement methods they don't need
/// - Clients are forced to depend on methods they don't use
/// - Not all heroes can heal, assassinate, gank, etc.
/// </summary>
public interface IHeroBefore
{
    void Heal();
    void Peel();
    void Assassinate();
    void Gank();
    void BasicAttack();
}