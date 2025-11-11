using MediatR;
using Microsoft.Extensions.Logging;

namespace Brew.Features.Patterns.Mediator.MediatR;

/// <summary>
/// Handles CreateOrderCommand
/// Each command has exactly ONE handler
/// </summary>
public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly ILogger<CreateOrderCommandHandler> _logger;
    private readonly IMediator _mediator;
    private readonly OrderRepository _repository;

    public CreateOrderCommandHandler(
        ILogger<CreateOrderCommandHandler> logger,
        IMediator mediator,
        OrderRepository repository)
    {
        _logger = logger;
        _mediator = mediator;
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[Command Handler] Processing CreateOrderCommand");
        _logger.LogInformation("  Product: {ProductName}, Quantity: {Quantity}, Price: ${Price:N2}",
            request.ProductName, request.Quantity, request.Price);

        // Create the order
        var orderId = Guid.NewGuid();
        var total = request.Quantity * request.Price;
        
        _repository.Add(orderId, new OrderDetails(
            orderId,
            request.ProductName,
            request.Quantity,
            request.Price,
            total,
            "Pending",
            DateTime.UtcNow
        ));

        _logger.LogInformation("  ✓ Order created with ID: {OrderId}, Total: ${Total:N2}", orderId, total);

        // Publish notification (decoupled - handlers will react independently)
        await _mediator.Publish(new OrderCreatedNotification(orderId, request.ProductName, total), cancellationToken);

        return orderId;
    }
}

/// <summary>
/// Handles CancelOrderCommand
/// </summary>
public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, bool>
{
    private readonly ILogger<CancelOrderCommandHandler> _logger;
    private readonly IMediator _mediator;
    private readonly OrderRepository _repository;

    public CancelOrderCommandHandler(
        ILogger<CancelOrderCommandHandler> logger,
        IMediator mediator,
        OrderRepository repository)
    {
        _logger = logger;
        _mediator = mediator;
        _repository = repository;
    }

    public async Task<bool> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[Command Handler] Processing CancelOrderCommand for Order: {OrderId}", request.OrderId);

        if (_repository.TryGet(request.OrderId, out var order))
        {
            _repository.Remove(request.OrderId);
            _logger.LogInformation("  ✓ Order {OrderId} cancelled", request.OrderId);

            await _mediator.Publish(new OrderCancelledNotification(request.OrderId), cancellationToken);
            return true;
        }

        _logger.LogWarning("  ✗ Order {OrderId} not found", request.OrderId);
        return false;
    }
}

/// <summary>
/// Handles UpdateOrderStatusCommand
/// </summary>
public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand, bool>
{
    private readonly ILogger<UpdateOrderStatusCommandHandler> _logger;
    private readonly IMediator _mediator;
    private readonly OrderRepository _repository;

    public UpdateOrderStatusCommandHandler(
        ILogger<UpdateOrderStatusCommandHandler> logger,
        IMediator mediator,
        OrderRepository repository)
    {
        _logger = logger;
        _mediator = mediator;
        _repository = repository;
    }

    public async Task<bool> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[Command Handler] Processing UpdateOrderStatusCommand for Order: {OrderId}", request.OrderId);

        if (_repository.TryGet(request.OrderId, out var order))
        {
            var oldStatus = order.Status;
            var updatedOrder = order with { Status = request.Status };
            _repository.Update(request.OrderId, updatedOrder);
            
            _logger.LogInformation("  ✓ Order {OrderId} status updated: {OldStatus} → {NewStatus}", 
                request.OrderId, oldStatus, request.Status);

            await _mediator.Publish(
                new OrderStatusChangedNotification(request.OrderId, oldStatus, request.Status), 
                cancellationToken);
            
            return true;
        }

        _logger.LogWarning("  ✗ Order {OrderId} not found", request.OrderId);
        return false;
    }
}
