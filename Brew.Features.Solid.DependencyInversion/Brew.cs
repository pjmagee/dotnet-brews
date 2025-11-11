using Brew.Features.Solid.DependencyInversion.After;
using Brew.Features.Solid.DependencyInversion.Before;
using Brew.Features.Solid.DependencyInversion.Shared;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Brew.Features.Solid.DependencyInversion;

public class Brew : ModuleBase
{
    protected override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        // Register the abstraction (interface) with its implementation
        // This is how DI containers enable Dependency Inversion
        services.AddSingleton<ICollection<Order>>(new List<Order>());
        services.AddSingleton<IOrderRepository, OrderRepository>();
        services.AddTransient<OrderServiceFlexible>();
    }

    protected override Task ExecuteAsync(CancellationToken token = default)
    {
        Console.WriteLine("=== Dependency Inversion Principle Demo ===\n");
        
        // BEFORE: Violates DIP - tightly coupled to concrete class
        Console.WriteLine("BEFORE (Violates DIP):");
        Console.WriteLine("- OrderServiceConcrete creates OrderRepository internally");
        Console.WriteLine("- High-level depends on low-level (tightly coupled)");
        var serviceConcrete = new OrderServiceConcrete();
        serviceConcrete.AddOrder(new Order { Id = 1, Price = 100m, Items = ["Item1"] });
        Console.WriteLine("✗ Cannot swap repository implementation\n");

        // AFTER: Follows DIP - depends on abstraction (IOrderRepository)
        Console.WriteLine("AFTER (Follows DIP):");
        Console.WriteLine("- OrderServiceFlexible depends on IOrderRepository (abstraction)");
        Console.WriteLine("- High-level and low-level both depend on abstraction");
        Console.WriteLine("- Implementation injected via DI container");
        
        // Get service from DI container (demonstrates proper dependency inversion)
        var serviceFlexible = Host.Services.GetRequiredService<OrderServiceFlexible>();
        serviceFlexible.AddOrder(new Order { Id = 2, Price = 200m, Items = ["Item2"] });
        Console.WriteLine("✓ Can easily swap to different IOrderRepository implementation");
        Console.WriteLine("✓ Easy to test with mocks");
        Console.WriteLine("✓ Loose coupling - changes to repository don't affect service\n");

        return Task.CompletedTask;
    }
}