namespace Brew.Features.ImmutableRecords;

public class ReadOnlyPerson(string name, DateTime dob)
{
    public string Name { get; } = name;
    public DateTime DateOfBirth { get; } = dob;
}