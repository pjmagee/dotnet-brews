using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brew.Features.Linq;

/// <summary>
/// Demonstrates comprehensive LINQ query patterns: filtering, projection, grouping, joins, aggregation.
/// </summary>
public class Brew : ModuleBase
{
    protected override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
    }

    protected override Task ExecuteAsync(CancellationToken token = default)
    {
        Logger.LogInformation("========== LINQ QUERY DEMONSTRATIONS ==========");
        Logger.LogInformation("Scenario: Product inventory and order analysis with LINQ\n");

        var products = GetSampleProducts();
        var orders = GetSampleOrders();

        DemonstrateFiltering(products);
        DemonstrateProjection(products);
        DemonstrateGrouping(products);
        DemonstrateJoins(products, orders);
        DemonstrateAggregation(products);
        DemonstrateComplexQueries(products, orders);

        Logger.LogInformation("\n---------- BENEFITS OF LINQ ----------");
        Logger.LogInformation("✓ Declarative syntax (what, not how)");
        Logger.LogInformation("✓ Type-safe queries with IntelliSense");
        Logger.LogInformation("✓ Deferred execution (query on demand)");
        Logger.LogInformation("✓ Works with any IEnumerable<T> source");
        Logger.LogInformation("✓ Composable queries (chain operators)");

        return Task.CompletedTask;
    }

    private void DemonstrateFiltering(List<Product> products)
    {
        Logger.LogInformation("---------- Filtering (Where) ----------");

        var expensiveProducts = products.Where(p => p.Price > 500).ToList();
        Logger.LogInformation("  Products > $500: {Count}", expensiveProducts.Count);

        var electronicsLowStock = products
            .Where(p => p.Category == "Electronics" && p.Stock < 20)
            .ToList();
        Logger.LogInformation("  Electronics with low stock: {Count}", electronicsLowStock.Count);
        foreach (var p in electronicsLowStock)
        {
            Logger.LogInformation("    {Name}: {Stock} units", p.Name, p.Stock);
        }
    }

    private void DemonstrateProjection(List<Product> products)
    {
        Logger.LogInformation("\n---------- Projection (Select) ----------");

        var productNames = products.Select(p => p.Name).ToList();
        Logger.LogInformation("  All product names: {Names}", string.Join(", ", productNames));

        var productSummaries = products
            .Select(p => new { p.Name, p.Price, StockStatus = p.Stock < 20 ? "Low" : "Normal" })
            .ToList();
        Logger.LogInformation("  Product summaries created: {Count}", productSummaries.Count);
        Logger.LogInformation("    Example: {Name} - ${Price} ({Status})", 
            productSummaries[0].Name, productSummaries[0].Price, productSummaries[0].StockStatus);
    }

    private void DemonstrateGrouping(List<Product> products)
    {
        Logger.LogInformation("\n---------- Grouping (GroupBy) ----------");

        var byCategory = products.GroupBy(p => p.Category);
        Logger.LogInformation("  Products grouped by category:");
        foreach (var group in byCategory)
        {
            Logger.LogInformation("    {Category}: {Count} products, Avg Price: ${AvgPrice:F2}",
                group.Key, group.Count(), group.Average(p => p.Price));
        }

        var stockGroups = products
            .GroupBy(p => p.Stock >= 20 ? "Well Stocked" : "Low Stock");
        Logger.LogInformation("  Stock status grouping:");
        foreach (var group in stockGroups)
        {
            Logger.LogInformation("    {Status}: {Count} products", group.Key, group.Count());
        }
    }

    private void DemonstrateJoins(List<Product> products, List<Order> orders)
    {
        Logger.LogInformation("\n---------- Joins (Join) ----------");

        var productOrders = orders
            .Join(products,
                order => order.ProductId,
                product => product.Id,
                (order, product) => new
                {
                    OrderId = order.Id,
                    ProductName = product.Name,
                    Quantity = order.Quantity,
                    TotalPrice = product.Price * order.Quantity
                })
            .ToList();

        Logger.LogInformation("  Order details with product info:");
        foreach (var po in productOrders.Take(3))
        {
            Logger.LogInformation("    Order #{OrderId}: {Qty}x {Product} = ${Total:F2}",
                po.OrderId, po.Quantity, po.ProductName, po.TotalPrice);
        }
    }

    private void DemonstrateAggregation(List<Product> products)
    {
        Logger.LogInformation("\n---------- Aggregation (Sum, Average, Min, Max) ----------");

        var totalInventoryValue = products.Sum(p => p.Price * p.Stock);
        Logger.LogInformation("  Total inventory value: ${Value:F2}", totalInventoryValue);

        var averagePrice = products.Average(p => p.Price);
        Logger.LogInformation("  Average product price: ${Price:F2}", averagePrice);

        var cheapest = products.MinBy(p => p.Price);
        var mostExpensive = products.MaxBy(p => p.Price);
        Logger.LogInformation("  Price range: ${Min:F2} ({MinProduct}) to ${Max:F2} ({MaxProduct})",
            cheapest?.Price, cheapest?.Name, mostExpensive?.Price, mostExpensive?.Name);
    }

    private void DemonstrateComplexQueries(List<Product> products, List<Order> orders)
    {
        Logger.LogInformation("\n---------- Complex Queries (Method Chaining) ----------");

        var topSellingProducts = orders
            .GroupBy(o => o.ProductId)
            .Select(g => new { ProductId = g.Key, TotalQuantity = g.Sum(o => o.Quantity) })
            .OrderByDescending(x => x.TotalQuantity)
            .Take(3)
            .Join(products,
                x => x.ProductId,
                p => p.Id,
                (x, p) => new { p.Name, x.TotalQuantity })
            .ToList();

        Logger.LogInformation("  Top 3 selling products:");
        foreach (var item in topSellingProducts)
        {
            Logger.LogInformation("    {Product}: {Qty} units sold", item.Name, item.TotalQuantity);
        }

        var recentHighValueOrders = orders
            .Where(o => o.OrderDate > DateTime.Now.AddDays(-30))
            .Join(products, o => o.ProductId, p => p.Id, (o, p) => new { Order = o, Product = p })
            .Where(x => x.Product.Price * x.Order.Quantity > 500)
            .OrderByDescending(x => x.Product.Price * x.Order.Quantity)
            .Select(x => new
            {
                OrderId = x.Order.Id,
                ProductName = x.Product.Name,
                Total = x.Product.Price * x.Order.Quantity
            })
            .ToList();

        Logger.LogInformation("  High-value orders (last 30 days): {Count}", recentHighValueOrders.Count);
    }

    private static List<Product> GetSampleProducts() =>
    [
        new() { Id = 1, Name = "Laptop", Price = 999.99m, Category = "Electronics", Stock = 15, CreatedDate = DateTime.Now.AddMonths(-6) },
        new() { Id = 2, Name = "Mouse", Price = 29.99m, Category = "Electronics", Stock = 50, CreatedDate = DateTime.Now.AddMonths(-3) },
        new() { Id = 3, Name = "Desk Chair", Price = 199.99m, Category = "Furniture", Stock = 25, CreatedDate = DateTime.Now.AddMonths(-4) },
        new() { Id = 4, Name = "Monitor", Price = 349.99m, Category = "Electronics", Stock = 18, CreatedDate = DateTime.Now.AddMonths(-2) },
        new() { Id = 5, Name = "Desk Lamp", Price = 45.99m, Category = "Furniture", Stock = 30, CreatedDate = DateTime.Now.AddMonths(-1) },
        new() { Id = 6, Name = "Keyboard", Price = 79.99m, Category = "Electronics", Stock = 40, CreatedDate = DateTime.Now.AddMonths(-5) },
        new() { Id = 7, Name = "Bookshelf", Price = 149.99m, Category = "Furniture", Stock = 12, CreatedDate = DateTime.Now.AddMonths(-7) }
    ];

    private static List<Order> GetSampleOrders() =>
    [
        new() { Id = 1, ProductId = 1, Quantity = 2, OrderDate = DateTime.Now.AddDays(-10) },
        new() { Id = 2, ProductId = 2, Quantity = 5, OrderDate = DateTime.Now.AddDays(-20) },
        new() { Id = 3, ProductId = 4, Quantity = 3, OrderDate = DateTime.Now.AddDays(-5) },
        new() { Id = 4, ProductId = 1, Quantity = 1, OrderDate = DateTime.Now.AddDays(-15) },
        new() { Id = 5, ProductId = 6, Quantity = 4, OrderDate = DateTime.Now.AddDays(-8) },
        new() { Id = 6, ProductId = 3, Quantity = 2, OrderDate = DateTime.Now.AddDays(-25) },
        new() { Id = 7, ProductId = 5, Quantity = 6, OrderDate = DateTime.Now.AddDays(-3) }
    ];
}
