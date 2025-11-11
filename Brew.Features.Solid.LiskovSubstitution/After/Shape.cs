namespace Brew.Features.Solid.LiskovSubstitution.After;

/// <summary>
/// FOLLOWS Liskov Substitution Principle:
/// - Abstraction for shapes that have area
/// - No assumptions about how dimensions relate to each other
/// - Subtypes can be substituted without breaking behavior
/// </summary>
public abstract class Shape
{
    public abstract int GetArea();
    public abstract string GetDescription();
}