using System;
namespace Brew.SOLID;

public class LiskovSubstitutionPrinciple : IBrew
{
    #region Before

    public void Before()
    {
        // this is possible with no liskov substitution principle
        Apple apple = new Orange();
        Console.WriteLine(apple.Color); // This will be red, but we know its wrong. Poor abstraction and inheritance
        apple.Bite();

        Orange orange = new Orange();
        orange.Squeeze(); // Only possible because not on base class. But also...you dont squeese all fruit, so why inherit from an apple?
    }

    public class Apple
    {
        public string Color => "Red";

        public void Bite()
        {
            // You bite apples for sure
        }
    }

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

    #endregion

    #region After

    // We define a structured base class that has shared attributes among all fruit
    public abstract class Fruit
    {
        // All fruit have colour
        public abstract string Color { get; }

        // You can cut all fruit 
        public abstract void Cut();
    }

    // no matter which fruit inherits from the base fruit, they will all behave correctly
    public class OrangeFruit : Fruit
    {
        public override string Color => "Orange";

        public override void Cut()
        {

        }
    }

    public class AppleFruit : Fruit
    {
        public override string Color => "Red";

        public override void Cut()
        {

        }
    }

    public void After()
    {
        Fruit orange = new OrangeFruit();
        Fruit apple = new AppleFruit();

        Console.WriteLine(orange.Color); // will be Orange
        Console.WriteLine(apple.Color); // will be Red
    }

    #endregion
}
