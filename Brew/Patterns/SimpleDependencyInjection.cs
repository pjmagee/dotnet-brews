
using System;

namespace Brew.Patterns;

/*
 * Dependency Injection is the concept of providing a dependency to a class via a property or constructor
 * Instead of the class itself instantiating what it requires, it is passed to it.
 *
 * This allows flexible behaviour when passing in abstract instances, i.e abstract class or interface
 */
public class SimpleDependencyInjection : IBrew
{
    public void Before()
    {
        ConcreteCar car = new ConcreteCar();
        car.Drive();
    }

    public void After()
    {
        FlexibleCar car = new FlexibleCar(new ShinyWheels(), new FastEngine(), new SportsChassis());
        car.Drive();
    }

    public abstract class Car
    {
        protected Wheels wheels;
        protected Engine engine;
        protected Chassis chassis;

        protected Car()
        {

        }

        public void Drive()
        {
            // logic with wheels, engine, chassis
        }
    }

    public sealed class ConcreteCar : Car
    {
        public ConcreteCar()
        {
            chassis = new Chassis();
            engine = new Engine();
            wheels = new Wheels();
            wheels = new Wheels();
        }
    }

    public sealed class FlexibleCar : Car
    {
        public FlexibleCar(Wheels wheels, Engine engine, Chassis chassis)
        {
            this.wheels = wheels;
            this.engine = engine;
            this.chassis = chassis;
        }
    }

    public class Wheels
    {
        public Wheels()
        {
            Console.WriteLine(GetType());
        }
    }

    public class ShinyWheels : Wheels
    {

    }

    public class Engine
    {
        public Engine()
        {
            Console.WriteLine(GetType());
        }
    }

    public class FastEngine : Engine
    {

    }

    public class Chassis
    {
        public Chassis()
        {
            Console.WriteLine(GetType());
        }
    }

    public class SportsChassis : Chassis
    {

    }
}
