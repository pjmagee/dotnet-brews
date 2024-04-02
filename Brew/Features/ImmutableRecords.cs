
using System;

namespace Brew.Features;

public class ImmutableRecords : IBrew
{
    public class ReadOnlyPerson
    {
        public string Name { get; }
        public DateTime DoB { get; }

        public ReadOnlyPerson(string name, DateTime dob)
        {
            Name = name;
            DoB = dob;
        }
    }

    public void Before()
    {
        var person = new ReadOnlyPerson("Patrick", new DateTime(1990, 05, 08));
    }
    
    /*
     * New C#9 feature of records give a cleaner less boilerplate solution
     */
    public record Person(string Name, DateTime DoB);

    public void After()
    {
        var record = new Person("Patrick", new DateTime(2021, 05, 08));
        
        // Deconstruct
        var (x, y) = record;
    }
}