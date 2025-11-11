namespace Brew.Features.Patterns.Singleton;

/// <summary>
/// Regular class - new instance created each time
/// </summary>
public class Person
{
    public DateTime CreatedAt { get; } = DateTime.Now;
    public Guid Id { get; } = Guid.NewGuid();

    public Person()
    {
        Console.WriteLine($"[Person] New instance created - ID: {Id}, Time: {CreatedAt:HH:mm:ss.fff}");
    }

    public void Speak()
    {
        Console.WriteLine($"[Person] Hello! My ID is {Id}, created at {CreatedAt:HH:mm:ss.fff}");
    }
}