namespace Brew.Features.Patterns.Mapper;

/// <summary>
/// Data Transfer Object (DTO) - simplified representation for API responses
/// This is what we send to external consumers
/// </summary>
public class CustomerDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Location { get; set; } = string.Empty;
    public int TotalOrders { get; set; }
    public decimal TotalSpent { get; set; }
    public DateTime MemberSince { get; set; }

    /// <summary>
    /// Mapper method - transforms domain model to DTO
    /// Benefits:
    /// - Hides internal implementation details
    /// - Flattens complex object graphs
    /// - Computes derived values
    /// - Controls what data is exposed
    /// </summary>
    public static CustomerDto MapFrom(Customer customer)
    {
        return new CustomerDto
        {
            Id = customer.Id,
            FullName = customer.GetFullName(), // Computed property
            Email = customer.Email,
            Age = customer.GetAge(), // Computed from DateOfBirth
            Location = $"{customer.HomeAddress.City}, {customer.HomeAddress.State}", // Flattened
            TotalOrders = customer.Orders.Count,
            TotalSpent = customer.GetTotalSpent(), // Aggregated calculation
            MemberSince = customer.CreatedAt
            // Note: CreditLimit is NOT mapped - sensitive data stays internal
        };
    }
}

/// <summary>
/// Detailed DTO for admin views - includes more information
/// </summary>
public class CustomerDetailDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public AddressDto Address { get; set; } = new();
    public List<OrderSummaryDto> RecentOrders { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime? LastModifiedAt { get; set; }

    public static CustomerDetailDto MapFrom(Customer customer)
    {
        return new CustomerDetailDto
        {
            Id = customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            DateOfBirth = customer.DateOfBirth,
            Address = AddressDto.MapFrom(customer.HomeAddress),
            RecentOrders = customer.Orders
                .OrderByDescending(o => o.OrderDate)
                .Take(5)
                .Select(OrderSummaryDto.MapFrom)
                .ToList(),
            CreatedAt = customer.CreatedAt,
            LastModifiedAt = customer.LastModifiedAt
        };
    }
}

public class AddressDto
{
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;

    public static AddressDto MapFrom(Address address)
    {
        return new AddressDto
        {
            Street = address.Street,
            City = address.City,
            State = address.State,
            ZipCode = address.ZipCode
        };
    }
}

public class OrderSummaryDto
{
    public Guid Id { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; } = string.Empty;

    public static OrderSummaryDto MapFrom(Order order)
    {
        return new OrderSummaryDto
        {
            Id = order.Id,
            OrderDate = order.OrderDate,
            Amount = order.TotalAmount,
            Status = order.Status
        };
    }
}