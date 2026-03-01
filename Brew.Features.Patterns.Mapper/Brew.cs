using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brew.Features.Patterns.Mapper;

/// <summary>
/// Demonstrates the Mapper Pattern - transforming domain models to DTOs
/// Use Case: API responses that expose simplified, secure data representations
/// </summary>
public class Brew : ModuleBase
{
    protected override void ConfigureServices(
        HostBuilderContext hostContext,
        IServiceCollection services
    ) { }

    protected override async Task ExecuteAsync(CancellationToken token = default)
    {
        Logger.LogInformation("=== MAPPER PATTERN DEMONSTRATION ===\n");
        Logger.LogInformation(
            "Scenario: Converting rich domain models to lightweight DTOs for API responses\n"
        );

        // Create sample domain model with complex internal state
        var customer = new Customer
        {
            FirstName = "Sarah",
            LastName = "Connor",
            Email = "sarah.connor@skynet.com",
            DateOfBirth = new DateTime(1985, 5, 15),
            HomeAddress = new Address
            {
                Street = "1234 Tech Boulevard",
                City = "Los Angeles",
                State = "CA",
                ZipCode = "90001",
                Country = "USA",
            },
            Orders = new List<Order>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    OrderDate = DateTime.UtcNow.AddDays(-30),
                    TotalAmount = 599.99m,
                    Status = "Delivered",
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    OrderDate = DateTime.UtcNow.AddDays(-15),
                    TotalAmount = 129.50m,
                    Status = "Delivered",
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    OrderDate = DateTime.UtcNow.AddDays(-5),
                    TotalAmount = 299.00m,
                    Status = "Shipped",
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    OrderDate = DateTime.UtcNow.AddDays(-2),
                    TotalAmount = 89.99m,
                    Status = "Processing",
                },
            },
        };

        Logger.LogInformation("╔════════════════════════════════════════════════════════════╗");
        Logger.LogInformation("║  DOMAIN MODEL (Internal Use)                              ║");
        Logger.LogInformation("╚════════════════════════════════════════════════════════════╝\n");

        Logger.LogInformation("Rich Customer Domain Model:");
        Logger.LogInformation("├─ Id: {Id}", customer.Id);
        Logger.LogInformation(
            "├─ Name: {FirstName} {LastName}",
            customer.FirstName,
            customer.LastName
        );
        Logger.LogInformation("├─ Email: {Email}", customer.Email);
        Logger.LogInformation("├─ Date of Birth: {DateOfBirth:yyyy-MM-dd}", customer.DateOfBirth);
        Logger.LogInformation("├─ Age (computed): {Age} years", customer.GetAge());
        Logger.LogInformation(
            "├─ Credit Limit (sensitive): ${CreditLimit:N2} ⚠️ PRIVATE",
            customer.GetType().GetProperty("CreditLimit")?.GetValue(customer)
        );
        Logger.LogInformation("├─ Address:");
        Logger.LogInformation("│  ├─ {Street}", customer.HomeAddress.Street);
        Logger.LogInformation(
            "│  └─ {City}, {State} {ZipCode}",
            customer.HomeAddress.City,
            customer.HomeAddress.State,
            customer.HomeAddress.ZipCode
        );
        Logger.LogInformation("├─ Orders ({Count}):", customer.Orders.Count);
        foreach (var order in customer.Orders)
        {
            Logger.LogInformation(
                "│  ├─ {OrderDate:yyyy-MM-dd}: ${Amount:N2} [{Status}]",
                order.OrderDate,
                order.TotalAmount,
                order.Status
            );
        }
        Logger.LogInformation(
            "├─ Total Spent (computed): ${TotalSpent:N2}",
            customer.GetTotalSpent()
        );
        Logger.LogInformation("└─ Member Since: {CreatedAt:yyyy-MM-dd}", customer.CreatedAt);

        await Task.Delay(500);

        Logger.LogInformation("\n╔════════════════════════════════════════════════════════════╗");
        Logger.LogInformation("║  MAPPER PATTERN - Transformation to DTOs                  ║");
        Logger.LogInformation("╚════════════════════════════════════════════════════════════╝\n");

        // Map to simple DTO for public API
        Logger.LogInformation("🔄 Mapping to CustomerDto (Public API)...\n");
        var customerDto = CustomerDto.MapFrom(customer);

        Logger.LogInformation("CustomerDto (Simplified for External API):");
        Logger.LogInformation("├─ Id: {Id}", customerDto.Id);
        Logger.LogInformation(
            "├─ Full Name: {FullName} (flattened from FirstName + LastName)",
            customerDto.FullName
        );
        Logger.LogInformation("├─ Email: {Email}", customerDto.Email);
        Logger.LogInformation("├─ Age: {Age} years (computed from DateOfBirth)", customerDto.Age);
        Logger.LogInformation(
            "├─ Location: {Location} (flattened from Address)",
            customerDto.Location
        );
        Logger.LogInformation(
            "├─ Total Orders: {TotalOrders} (aggregated)",
            customerDto.TotalOrders
        );
        Logger.LogInformation(
            "├─ Total Spent: ${TotalSpent:N2} (aggregated)",
            customerDto.TotalSpent
        );
        Logger.LogInformation("└─ Member Since: {MemberSince:yyyy-MM-dd}", customerDto.MemberSince);
        Logger.LogInformation("\n✓ Notice: CreditLimit is NOT exposed (security)");
        Logger.LogInformation(
            "✓ Notice: Complex Address object flattened to simple Location string"
        );
        Logger.LogInformation("✓ Notice: Order collection aggregated to counts and totals");

        await Task.Delay(500);

        // Map to detailed DTO for admin API
        Logger.LogInformation("\n🔄 Mapping to CustomerDetailDto (Admin API)...\n");
        var detailDto = CustomerDetailDto.MapFrom(customer);

        Logger.LogInformation("CustomerDetailDto (Detailed for Admin Interface):");
        Logger.LogInformation("├─ Id: {Id}", detailDto.Id);
        Logger.LogInformation(
            "├─ Name: {FirstName} {LastName}",
            detailDto.FirstName,
            detailDto.LastName
        );
        Logger.LogInformation("├─ Email: {Email}", detailDto.Email);
        Logger.LogInformation("├─ Date of Birth: {DateOfBirth:yyyy-MM-dd}", detailDto.DateOfBirth);
        Logger.LogInformation("├─ Address:");
        Logger.LogInformation("│  ├─ {Street}", detailDto.Address.Street);
        Logger.LogInformation(
            "│  └─ {City}, {State} {ZipCode}",
            detailDto.Address.City,
            detailDto.Address.State,
            detailDto.Address.ZipCode
        );
        Logger.LogInformation(
            "├─ Recent Orders ({Count}, showing top 5):",
            detailDto.RecentOrders.Count
        );
        foreach (var order in detailDto.RecentOrders)
        {
            Logger.LogInformation(
                "│  ├─ {OrderDate:yyyy-MM-dd}: ${Amount:N2} [{Status}]",
                order.OrderDate,
                order.Amount,
                order.Status
            );
        }
        Logger.LogInformation("└─ Member Since: {CreatedAt:yyyy-MM-dd}", detailDto.CreatedAt);

        Logger.LogInformation("\n" + new string('═', 60));
        Logger.LogInformation("=== KEY BENEFITS OF MAPPER PATTERN ===");
        Logger.LogInformation(new string('═', 60));
        Logger.LogInformation("✓ SECURITY: Hide sensitive internal data (e.g., CreditLimit)");
        Logger.LogInformation(
            "✓ SIMPLIFICATION: Flatten complex object graphs for easier consumption"
        );
        Logger.LogInformation("✓ VERSIONING: Multiple DTOs for different API versions/consumers");
        Logger.LogInformation("✓ PERFORMANCE: Send only necessary data over the wire");
        Logger.LogInformation("✓ DECOUPLING: API contracts independent of domain model changes");
        Logger.LogInformation("✓ COMPUTED VALUES: Include derived/aggregated data");
        Logger.LogInformation(
            "\nUse Cases: REST APIs, gRPC services, message queues, external integrations"
        );
        Logger.LogInformation("Popular libraries: AutoMapper, Mapster, TinyMapper");
    }
}
