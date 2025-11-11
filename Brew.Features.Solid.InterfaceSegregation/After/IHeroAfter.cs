namespace Brew.Features.Solid.InterfaceSegregation.After;

/// <summary>
/// FOLLOWS Interface Segregation Principle:
/// - Small, focused interface with only essential common behavior
/// - All heroes can perform basic attack
/// - Specific capabilities separated into role-specific interfaces
/// </summary>
public interface IHeroAfter
{
    void BasicAttack();
}