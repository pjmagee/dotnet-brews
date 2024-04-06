namespace Brew.Features.CQRS.Simple.Queries;

public class GetDataQueryHandler(List<string> dataStore) : IQueryHandler<GetDataQuery, IEnumerable<string>>
{
    public Task<IEnumerable<string>> HandleAsync(GetDataQuery query, CancellationToken cancellationToken)
    {
        return Task.FromResult<IEnumerable<string>>(dataStore);
    }
}