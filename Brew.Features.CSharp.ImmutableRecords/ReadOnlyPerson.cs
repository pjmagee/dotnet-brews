namespace Brew.Features.ImmutableRecords;

// Traditional class approach (before records)
public class ReadOnlyPerson(string name, DateTime dob)
{
    public string Name { get; } = name;
    public DateTime DateOfBirth { get; } = dob;
    
    // Manual Equals/GetHashCode required for value equality
    // No with-expressions, no deconstruction out of the box
}

// Mutable class for comparison (reference equality)
public class MutablePersonClass(string name, DateTime dob)
{
    public string Name { get; set; } = name;
    public DateTime DateOfBirth { get; set; } = dob;
    
    // Reference equality by default (two instances with same data != equal)
}
