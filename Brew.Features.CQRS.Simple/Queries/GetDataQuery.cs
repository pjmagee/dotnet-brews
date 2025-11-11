using Brew.Features.CQRS.Simple.Models;

namespace Brew.Features.CQRS.Simple.Queries;

public class GetProductSummariesQuery : IQuery<IEnumerable<ProductSummary>>
{
    // No parameters - fetch all
}
