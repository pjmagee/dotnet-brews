namespace Brew.Features.Patterns.Singleton;

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