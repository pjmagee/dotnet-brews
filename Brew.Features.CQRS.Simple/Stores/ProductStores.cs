using System.Collections.Concurrent;
using Brew.Features.CQRS.Simple.Models;

namespace Brew.Features.CQRS.Simple.Stores;

// Write store: normalized, focused on transactional consistency
public class ProductWriteStore
{
    private readonly ConcurrentDictionary<int, Product> _products = new();

    public void Add(Product product)
    {
        _products[product.Id] = product;
    }

    public Product? Get(int id)
    {
        _products.TryGetValue(id, out var product);
        return product;
    }

    public void Update(int id, Action<Product> updateAction)
    {
        if (_products.TryGetValue(id, out var product))
        {
            updateAction(product);
        }
    }

    public IEnumerable<Product> GetAll() => _products.Values;
}

// Read store: denormalized, optimized for fast queries
public class ProductReadStore
{
    private readonly ConcurrentDictionary<int, ProductSummary> _summaries = new();

    public void Sync(Product product)
    {
        // Project from write model to read model
        _summaries[product.Id] = new ProductSummary
        {
            Id = product.Id,
            Name = product.Name,
            Stock = product.Stock,
            Price = product.Price,
            IsActive = product.IsActive
        };
    }

    public IEnumerable<ProductSummary> GetAll() => _summaries.Values;

    public IEnumerable<ProductSummary> GetLowStock(int threshold) =>
        _summaries.Values.Where(p => p.IsActive && p.Stock <= threshold);
}
