using Brew.Features.CQRS.Simple.Models;
using Brew.Features.CQRS.Simple.Stores;
using Microsoft.Extensions.Logging;

namespace Brew.Features.CQRS.Simple.Queries;

public class GetProductSummariesQueryHandler(
    ProductReadStore readStore,
    ILogger<GetProductSummariesQueryHandler> logger) : IQueryHandler<GetProductSummariesQuery, IEnumerable<ProductSummary>>
{
    public Task<IEnumerable<ProductSummary>> HandleAsync(GetProductSummariesQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("[QUERY] Fetching all product summaries from read store");
        var summaries = readStore.GetAll();
        logger.LogInformation("[QUERY] Retrieved {Count} product(s)", summaries.Count());
        return Task.FromResult(summaries);
    }
}
