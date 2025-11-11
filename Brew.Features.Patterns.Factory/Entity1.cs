using Microsoft.Extensions.Logging;

namespace Brew.Features.Patterns.Factory;

/// <summary>
/// Email notification implementation
/// </summary>
public class EmailNotification : INotification
{
    private readonly ILogger<EmailNotification> _logger;
    
    public string Type => "Email";

    public EmailNotification(ILogger<EmailNotification> logger)
    {
        _logger = logger;
    }

    public async Task SendAsync(string recipient, string subject, string message)
    {
        _logger.LogInformation("[{Type}] Preparing to send...", Type);
        _logger.LogInformation("  To: {Recipient}", recipient);
        _logger.LogInformation("  Subject: {Subject}", subject);
        _logger.LogInformation("  Connecting to SMTP server...");
        await Task.Delay(100); // Simulate SMTP connection
        _logger.LogInformation("  Sending email via SMTP...");
        await Task.Delay(150); // Simulate sending
        _logger.LogInformation("  ✓ Email sent successfully to {Recipient}", recipient);
    }
}

/// <summary>
/// SMS notification implementation
/// </summary>
public class SmsNotification : INotification
{
    private readonly ILogger<SmsNotification> _logger;
    
    public string Type => "SMS";

    public SmsNotification(ILogger<SmsNotification> logger)
    {
        _logger = logger;
    }

    public async Task SendAsync(string recipient, string subject, string message)
    {
        _logger.LogInformation("[{Type}] Preparing to send...", Type);
        _logger.LogInformation("  To: {Recipient}", recipient);
        _logger.LogInformation("  Message: {Message} (max 160 chars)", message.Length > 50 ? message.Substring(0, 50) + "..." : message);
        _logger.LogInformation("  Connecting to SMS gateway...");
        await Task.Delay(80); // Simulate gateway connection
        _logger.LogInformation("  Sending SMS...");
        await Task.Delay(120); // Simulate sending
        _logger.LogInformation("  ✓ SMS sent successfully to {Recipient}", recipient);
    }
}

/// <summary>
/// Push notification implementation
/// </summary>
public class PushNotification : INotification
{
    private readonly ILogger<PushNotification> _logger;
    
    public string Type => "Push";

    public PushNotification(ILogger<PushNotification> logger)
    {
        _logger = logger;
    }

    public async Task SendAsync(string recipient, string subject, string message)
    {
        _logger.LogInformation("[{Type}] Preparing to send...", Type);
        _logger.LogInformation("  To device: {Recipient}", recipient);
        _logger.LogInformation("  Title: {Subject}", subject);
        _logger.LogInformation("  Connecting to push notification service (FCM/APNS)...");
        await Task.Delay(60); // Simulate service connection
        _logger.LogInformation("  Sending push notification...");
        await Task.Delay(90); // Simulate sending
        _logger.LogInformation("  ✓ Push notification sent successfully to device {Recipient}", recipient);
    }
}

/// <summary>
/// Slack notification implementation
/// </summary>
public class SlackNotification : INotification
{
    private readonly ILogger<SlackNotification> _logger;
    
    public string Type => "Slack";

    public SlackNotification(ILogger<SlackNotification> logger)
    {
        _logger = logger;
    }

    public async Task SendAsync(string recipient, string subject, string message)
    {
        _logger.LogInformation("[{Type}] Preparing to send...", Type);
        _logger.LogInformation("  To channel: {Recipient}", recipient);
        _logger.LogInformation("  Subject: {Subject}", subject);
        _logger.LogInformation("  Connecting to Slack webhook...");
        await Task.Delay(70); // Simulate webhook call
        _logger.LogInformation("  Posting message to Slack...");
        await Task.Delay(100); // Simulate posting
        _logger.LogInformation("  ✓ Slack message posted successfully to {Recipient}", recipient);
    }
}