using Microsoft.Extensions.Logging;

namespace Brew.Features.Patterns.Factory;

/// <summary>
/// BEFORE: Tight coupling - consumer creates concrete notification types directly
/// Problems:
/// - Consumer must know all concrete classes
/// - Hard to add new notification types
/// - Difficult to test (hard to mock)
/// - Violates Open/Closed Principle
/// </summary>
public class NotificationServiceBefore
{
    private readonly ILogger<NotificationServiceBefore> _logger;

    public NotificationServiceBefore(ILogger<NotificationServiceBefore> logger)
    {
        _logger = logger;
    }

    public async Task SendNotificationAsync(string type, string recipient, string subject, string message)
    {
        _logger.LogWarning("❌ BEFORE FACTORY - Tightly coupled implementation");
        
        // Consumer must know about all concrete types
        if (type == "email")
        {
            var notification = new EmailNotification(_logger as ILogger<EmailNotification> 
                ?? throw new InvalidOperationException("Logger type mismatch"));
            await notification.SendAsync(recipient, subject, message);
        }
        else if (type == "sms")
        {
            var notification = new SmsNotification(_logger as ILogger<SmsNotification> 
                ?? throw new InvalidOperationException("Logger type mismatch"));
            await notification.SendAsync(recipient, subject, message);
        }
        else if (type == "push")
        {
            var notification = new PushNotification(_logger as ILogger<PushNotification> 
                ?? throw new InvalidOperationException("Logger type mismatch"));
            await notification.SendAsync(recipient, subject, message);
        }
        else
        {
            throw new ArgumentException($"Unknown notification type: {type}");
        }

        _logger.LogWarning("⚠️ Problems: Tight coupling, hard to extend, difficult to test\n");
    }
}