namespace Brew.Models;

public class Record : IRecord, IEquatable<Record>
{
    public Guid Id { get; }

    public bool Equals(Record other) => this.Id.Equals(other.Id);
}