namespace Brew.Features.Solid.LiskovSubstitution.After;

/// <summary>
/// Rectangle implementation that FOLLOWS LSP:
/// - Width and Height are independent
/// - No hidden side effects when setting properties
/// - Can be substituted wherever Shape is expected
/// </summary>
public class RectangleShape : Shape
{
    public int Width { get; set; }
    public int Height { get; set; }

    public override int GetArea() => Width * Height;

    public override string GetDescription() => $"Rectangle ({Width}x{Height})";
}