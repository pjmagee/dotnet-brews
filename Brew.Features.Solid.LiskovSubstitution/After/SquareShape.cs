namespace Brew.Features.Solid.LiskovSubstitution.After;

/// <summary>
/// Square implementation that FOLLOWS LSP:
/// - Doesn't inherit from RectangleShape (avoiding IS-A relationship)
/// - Both Square and Rectangle implement Shape independently
/// - Width and Height are always equal (enforced in constructor)
/// - Can be substituted wherever Shape is expected
/// - No behavioral surprises for clients
/// </summary>
public class SquareShape : Shape
{
    private int _side;

    public int Side
    {
        get => _side;
        set => _side = value;
    }

    public override int GetArea() => _side * _side;

    public override string GetDescription() => $"Square ({_side}x{_side})";
}