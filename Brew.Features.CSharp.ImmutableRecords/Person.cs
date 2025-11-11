namespace Brew.Features.ImmutableRecords;

// Modern record: immutable, value equality, with-expressions, deconstruction
public record Person(string Name, DateTime DateOfBirth);

// Record with additional members
public record Employee(string Name, DateTime DateOfBirth, string Department, decimal Salary) : Person(Name, DateOfBirth)
{
    // Can add custom methods, properties
    public string DisplayName => $"{Name} ({Department})";
    
    // Can override default ToString
    public override string ToString() => $"{Name} works in {Department}";
}
