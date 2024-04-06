namespace Brew.Features.Solid.LiskovSubstitution.Before;

public class Orange : Apple
{
    public Orange()
    {
        // We inherit from Apple which has a function of Bite..but we dont bite oranges...
        // Just because Oranges are also a fruit, it does not mean that we can substitute an apple for an orange 
    }

    public void Squeeze()
    {
        // You dont squeeze apples...
    }
}