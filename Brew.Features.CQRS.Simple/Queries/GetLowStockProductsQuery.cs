using Brew.Features.CQRS.Simple.Models;

namespace Brew.Features.CQRS.Simple.Queries;

public class GetLowStockProductsQuery(int threshold = 10) : IQuery<IEnumerable<ProductSummary>>
{
    public int Threshold { get; } = threshold;
}
