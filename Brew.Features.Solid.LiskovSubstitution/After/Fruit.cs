namespace Brew.Features.Solid.LiskovSubstitution.After;

public abstract class Fruit
{
    // All fruit have colour
    public abstract string Colour { get; }

    // You can cut all fruit 
    public abstract void Cut();
}