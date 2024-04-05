namespace Brew.Features.ImmutableRecords;

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