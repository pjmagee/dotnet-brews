namespace Brew.Features.ImmutableRecords;

/*
 * New C#9 feature of records give a cleaner less boilerplate solution
 */

public class Brew : IBrew
{ 
    public void Before()
    {
        var person = new ReadOnlyPerson("Patrick", new DateTime(1990, 05, 08));
    }
    
    public void After()
    {
        var record = new Person("Patrick", new DateTime(2021, 05, 08));
        
        // Deconstruct
        var (x, y) = record;
    }
}