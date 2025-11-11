namespace Brew.Features.Patterns.Singleton;

/// <summary>
/// Singleton Pattern - Only ONE instance exists for the lifetime of the application
/// Thread-safe implementation using Lazy<T>
/// </summary>
public class SingletonPerson : Person
{
    // Thread-safe lazy initialization - the instance is created only when first accessed
    private static readonly Lazy<SingletonPerson> _instance = new(() =>
    {
        Console.WriteLine("[SingletonPerson] *** CREATING THE ONE AND ONLY INSTANCE ***");
        return new SingletonPerson();
    });

    public static SingletonPerson Instance => _instance.Value;

    // Private constructor prevents external instantiation
    private SingletonPerson()
    {
        Console.WriteLine("[SingletonPerson] Constructor called (this only happens ONCE)");
    }
}