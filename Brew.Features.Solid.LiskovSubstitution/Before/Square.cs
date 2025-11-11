namespace Brew.Features.Solid.LiskovSubstitution.Before;

/// <summary>
/// VIOLATES Liskov Substitution Principle:
/// - Square IS-A Rectangle mathematically, but NOT behaviorally in OOP
/// - Breaks Rectangle's contract: setting Width should NOT affect Height
/// - Cannot be substituted for Rectangle without breaking client code expectations
/// - Changing one dimension changes both (side effect not in parent contract)
/// </summary>
public class Square : Rectangle
{
    private int _side;

    // ❌ LSP Violation: Overriding setters to maintain square constraint
    // This changes the expected behavior from the base class
    public override int Width
    {
        get => _side;
        set
        {
            _side = value;
            // Side effect: Setting width also changes height (breaks parent's contract)
        }
    }

    public override int Height
    {
        get => _side;
        set
        {
            _side = value;
            // Side effect: Setting height also changes width (breaks parent's contract)
        }
    }
}