
using System;

namespace Brew.Features;

public class ImmutableRecords
{
    #region Without

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

    public static void Without()
    {
        var person = new ReadOnlyPerson("Patrick", new DateTime(1990, 05, 08));
    }

    #endregion

    #region With

    /*
     * New C#9 feature of records give a cleaner less boilerplate solution
     */
    public record Person(string Name, DateTime DoB);

    public static void With()
    {
        var record = new Person("Patrick", new DateTime(2021, 05, 08));
    }

    #endregion
}
