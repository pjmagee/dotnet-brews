using Microsoft.Extensions.Logging;

namespace Brew.Features.Patterns.Factory;

/// <summary>
/// Notification type enum
/// </summary>
public enum NotificationType
{
    Email,
    Sms,
    Push,
    Slack
}

/// <summary>
/// Factory Pattern - Creates notification objects based on type
/// Benefits:
/// - Centralizes object creation logic
/// - Client code doesn't need to know concrete class names
/// - Easy to add new notification types
/// - Supports dependency injection
/// </summary>
public class NotificationFactory
{
    private readonly ILoggerFactory _loggerFactory;

    public NotificationFactory(ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory;
    }

    /// <summary>
    /// Creates appropriate notification instance based on type
    /// </summary>
    public INotification Create(NotificationType type)
    {
        return type switch
        {
            NotificationType.Email => new EmailNotification(_loggerFactory.CreateLogger<EmailNotification>()),
            NotificationType.Sms => new SmsNotification(_loggerFactory.CreateLogger<SmsNotification>()),
            NotificationType.Push => new PushNotification(_loggerFactory.CreateLogger<PushNotification>()),
            NotificationType.Slack => new SlackNotification(_loggerFactory.CreateLogger<SlackNotification>()),
            _ => throw new ArgumentException($"Unknown notification type: {type}", nameof(type))
        };
    }

    /// <summary>
    /// Creates notification based on string configuration
    /// Useful for runtime configuration from files/database
    /// </summary>
    public INotification CreateFromConfig(string typeString)
    {
        if (Enum.TryParse<NotificationType>(typeString, ignoreCase: true, out var type))
        {
            return Create(type);
        }

        throw new ArgumentException($"Invalid notification type: {typeString}", nameof(typeString));
    }
}