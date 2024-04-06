namespace Brew.Features.Patterns.Singleton;

public class SingletonPerson : Person
{
    private static SingletonPerson? _me;

    public static SingletonPerson Me => _me ??= new SingletonPerson();

    // Cannot create
    private SingletonPerson()
    {

    }
}