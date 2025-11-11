using Brew.Features.CQRS.Simple.Models;
using Brew.Features.CQRS.Simple.Stores;
using Microsoft.Extensions.Logging;

namespace Brew.Features.CQRS.Simple.Queries;

public class GetLowStockProductsQueryHandler(
    ProductReadStore readStore,
    ILogger<GetLowStockProductsQueryHandler> logger) : IQueryHandler<GetLowStockProductsQuery, IEnumerable<ProductSummary>>
{
    public Task<IEnumerable<ProductSummary>> HandleAsync(GetLowStockProductsQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("[QUERY] Fetching low stock products (threshold: {Threshold})", query.Threshold);
        var lowStock = readStore.GetLowStock(query.Threshold);
        logger.LogInformation("[QUERY] Found {Count} low stock product(s)", lowStock.Count());
        return Task.FromResult(lowStock);
    }
}
