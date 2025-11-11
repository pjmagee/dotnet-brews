using Microsoft.Extensions.Logging;

namespace Brew.Features.Patterns.Factory;

/// <summary>
/// AFTER: Loose coupling - consumer uses factory to create notifications
/// Benefits:
/// - Consumer only knows about INotification interface
/// - Factory handles all creation logic
/// - Easy to add new types (just extend factory)
/// - Easy to test (can mock factory)
/// - Follows Open/Closed Principle
/// </summary>
public class NotificationServiceAfter
{
    private readonly NotificationFactory _factory;
    private readonly ILogger<NotificationServiceAfter> _logger;

    public NotificationServiceAfter(NotificationFactory factory, ILogger<NotificationServiceAfter> logger)
    {
        _factory = factory;
        _logger = logger;
    }

    public async Task SendNotificationAsync(NotificationType type, string recipient, string subject, string message)
    {
        _logger.LogInformation("✓ AFTER FACTORY - Loosely coupled implementation");
        
        // Consumer doesn't need to know about concrete types!
        // Factory handles all creation logic
        INotification notification = _factory.Create(type);
        
        await notification.SendAsync(recipient, subject, message);
        
        _logger.LogInformation("✓ Benefits: Loose coupling, easy to extend, testable\n");
    }

    public async Task SendNotificationFromConfigAsync(string typeConfig, string recipient, string subject, string message)
    {
        _logger.LogInformation("✓ FACTORY WITH CONFIG - Creating from configuration string");
        
        // Factory can even create from string configuration
        INotification notification = _factory.CreateFromConfig(typeConfig);
        
        await notification.SendAsync(recipient, subject, message);
    }
}