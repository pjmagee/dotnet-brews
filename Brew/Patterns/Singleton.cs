
using System;

namespace Brew.Patterns;

public class Singleton : IBrew
{
    public void Before()
    {
        Person person1 = new Person();
        person1.Speak();

        person1 = new Person();
        person1.Speak();
    }

    public void After()
    {
        SingletonPerson person = SingletonPerson.Me;
        person.Speak();

        person = SingletonPerson.Me; // Grab same single instance

        person.Speak(); // will be same age as first one
    }

    public class Person
    {
        public DateTime Age { get; } = DateTime.Now;

        public Person()
        {

        }

        public void Speak()
        {
            Console.WriteLine($"Born: {Age}");
        }
    }

    public class SingletonPerson : Person
    {
        private static SingletonPerson? _me;

        public static SingletonPerson Me => _me ??= new SingletonPerson();

        // Cannot create
        private SingletonPerson()
        {

        }
    }
}
