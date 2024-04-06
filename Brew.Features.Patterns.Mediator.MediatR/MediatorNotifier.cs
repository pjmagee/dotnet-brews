using MediatR;
using Microsoft.Extensions.Logging;

namespace Brew.Features.Patterns.Mediator.MediatR;

internal class MediatorNotifier(ILogger<MediatorNotifier> logger) : INotificationHandler<MediatorNotification>
{
    public Task Handle(MediatorNotification notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Mediator Event Handler called");
            
        return Task.CompletedTask;
    }
}