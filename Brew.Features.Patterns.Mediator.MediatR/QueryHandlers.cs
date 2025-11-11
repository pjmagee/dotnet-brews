using MediatR;
using Microsoft.Extensions.Logging;

namespace Brew.Features.Patterns.Mediator.MediatR;

/// <summary>
/// Handles GetOrderQuery
/// Queries only read data, no side effects
/// </summary>
public class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, OrderDetails?>
{
    private readonly ILogger<GetOrderQueryHandler> _logger;
    private readonly OrderRepository _repository;

    public GetOrderQueryHandler(ILogger<GetOrderQueryHandler> logger, OrderRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public Task<OrderDetails?> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[Query Handler] Processing GetOrderQuery for Order: {OrderId}", request.OrderId);

        if (_repository.TryGet(request.OrderId, out var order))
        {
            _logger.LogInformation("  ✓ Order found: {ProductName}, Status: {Status}", order.ProductName, order.Status);
            return Task.FromResult<OrderDetails?>(order);
        }

        _logger.LogWarning("  ✗ Order {OrderId} not found", request.OrderId);
        return Task.FromResult<OrderDetails?>(null);
    }
}

/// <summary>
/// Handles GetAllOrdersQuery
/// </summary>
public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, List<OrderDetails>>
{
    private readonly ILogger<GetAllOrdersQueryHandler> _logger;
    private readonly OrderRepository _repository;

    public GetAllOrdersQueryHandler(ILogger<GetAllOrdersQueryHandler> logger, OrderRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public Task<List<OrderDetails>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[Query Handler] Processing GetAllOrdersQuery");

        var orders = _repository.GetAll();
        _logger.LogInformation("  ✓ Found {Count} orders", orders.Count);

        return Task.FromResult(orders);
    }
}
