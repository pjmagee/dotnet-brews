namespace Brew.Features.Patterns.Mapper;

/// <summary>
/// Rich domain model with business logic and private state
/// This is what we use internally in our application
/// </summary>
public class Customer
{
    public Guid Id { get; private set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public Address HomeAddress { get; set; } = new();
    public List<Order> Orders { get; set; } = new();
    public decimal CreditLimit { get; private set; } // Sensitive data we don't expose
    public DateTime CreatedAt { get; private set; }
    public DateTime? LastModifiedAt { get; set; }

    public Customer()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }

    // Business logic
    public int GetAge() => DateTime.UtcNow.Year - DateOfBirth.Year;
    public decimal GetTotalSpent() => Orders.Sum(o => o.TotalAmount);
    public string GetFullName() => $"{FirstName} {LastName}";
}

public class Address
{
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
}

public class Order
{
    public Guid Id { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
}
