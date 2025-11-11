namespace Brew.Features.Solid.LiskovSubstitution.Before;

/// <summary>
/// Base class representing a Rectangle
/// Contract: Width and Height can be set independently
/// </summary>
public class Rectangle
{
    public virtual int Width { get; set; }
    public virtual int Height { get; set; }

    public int GetArea() => Width * Height;
}