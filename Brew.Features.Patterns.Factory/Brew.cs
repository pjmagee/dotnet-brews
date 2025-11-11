using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brew.Features.Patterns.Factory;

/// <summary>
/// Demonstrates the Factory Pattern - centralizing object creation
/// Use Case: Notification system supporting multiple channels (Email, SMS, Push, Slack)
/// </summary>
public class Brew : ModuleBase
{
    protected override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        services.AddSingleton<NotificationFactory>();
        services.AddSingleton<NotificationServiceBefore>();
        services.AddSingleton<NotificationServiceAfter>();
    }

    protected override async Task ExecuteAsync(CancellationToken token = default)
    {
        Logger.LogInformation("=== FACTORY PATTERN DEMONSTRATION ===\n");
        Logger.LogInformation("Scenario: Multi-channel notification system\n");

        Logger.LogInformation("╔════════════════════════════════════════════════════════════╗");
        Logger.LogInformation("║  PROBLEM: Object Creation Complexity                      ║");
        Logger.LogInformation("╚════════════════════════════════════════════════════════════╝\n");

        // Show the problem without factory
        var serviceBefore = Host.Services.GetRequiredService<NotificationServiceBefore>();
        
        Logger.LogInformation("Without Factory Pattern:");
        Logger.LogInformation("- Consumer must know all concrete notification types");
        Logger.LogInformation("- If-else or switch statements everywhere");
        Logger.LogInformation("- Adding new types requires changing consumer code");
        Logger.LogInformation("- Tight coupling between consumer and concrete types\n");

        await Task.Delay(300);

        Logger.LogInformation("╔════════════════════════════════════════════════════════════╗");
        Logger.LogInformation("║  SOLUTION: Factory Pattern                                ║");
        Logger.LogInformation("╚════════════════════════════════════════════════════════════╝\n");

        var factory = Host.Services.GetRequiredService<NotificationFactory>();
        var serviceAfter = Host.Services.GetRequiredService<NotificationServiceAfter>();

        Logger.LogInformation("With Factory Pattern:");
        Logger.LogInformation("- Factory centralizes creation logic");
        Logger.LogInformation("- Consumer only knows about INotification interface");
        Logger.LogInformation("- Adding new types only requires updating factory");
        Logger.LogInformation("- Loose coupling, easy to test\n");

        await Task.Delay(300);

        Logger.LogInformation("╔════════════════════════════════════════════════════════════╗");
        Logger.LogInformation("║  Testing All Notification Types                           ║");
        Logger.LogInformation("╚════════════════════════════════════════════════════════════╝\n");

        // Test all notification types through factory
        var notifications = new[]
        {
            (Type: NotificationType.Email, Recipient: "user@example.com", Subject: "Account Created", Message: "Welcome to our platform!"),
            (Type: NotificationType.Sms, Recipient: "+1-555-0123", Subject: "Security Alert", Message: "Your verification code is: 123456"),
            (Type: NotificationType.Push, Recipient: "device_abc123", Subject: "New Message", Message: "You have 3 new messages"),
            (Type: NotificationType.Slack, Recipient: "#engineering", Subject: "Deployment", Message: "Production deployment completed successfully")
        };

        foreach (var (type, recipient, subject, message) in notifications)
        {
            Logger.LogInformation(new string('=', 60));
            Logger.LogInformation("📬 Sending {Type} Notification", type);
            Logger.LogInformation(new string('=', 60));

            await serviceAfter.SendNotificationAsync(type, recipient, subject, message);

            await Task.Delay(200);
        }

        Logger.LogInformation("\n╔════════════════════════════════════════════════════════════╗");
        Logger.LogInformation("║  Runtime Configuration Support                            ║");
        Logger.LogInformation("╚════════════════════════════════════════════════════════════╝\n");

        Logger.LogInformation("Factory can create notifications from configuration strings:");
        Logger.LogInformation("(Useful for reading from config files, database, etc.)\n");

        var configTypes = new[] { "Email", "SMS", "Push", "Slack" };
        
        foreach (var configType in configTypes)
        {
            Logger.LogInformation("Configuration: NotificationType = '{ConfigType}'", configType);
            
            try
            {
                var notification = factory.CreateFromConfig(configType);
                Logger.LogInformation("  ✓ Created: {Type} notification\n", notification.Type);
            }
            catch (ArgumentException ex)
            {
                Logger.LogError("  ✗ Failed: {Message}\n", ex.Message);
            }

            await Task.Delay(100);
        }

        Logger.LogInformation(new string('═', 60));
        Logger.LogInformation("=== KEY BENEFITS OF FACTORY PATTERN ===");
        Logger.LogInformation(new string('═', 60));
        Logger.LogInformation("✓ ENCAPSULATION: Creation logic centralized in one place");
        Logger.LogInformation("✓ LOOSE COUPLING: Client doesn't depend on concrete classes");
        Logger.LogInformation("✓ EXTENSIBILITY: Easy to add new product types");
        Logger.LogInformation("✓ TESTABILITY: Can mock factory for unit tests");
        Logger.LogInformation("✓ CONFIGURATION: Support runtime type selection");
        Logger.LogInformation("✓ CONSISTENCY: Ensures objects created correctly");
        Logger.LogInformation("\nUse Cases: Plugin systems, document parsers, notification services, payment gateways");
        Logger.LogInformation("Related Patterns: Abstract Factory, Builder, Prototype");
    }
}