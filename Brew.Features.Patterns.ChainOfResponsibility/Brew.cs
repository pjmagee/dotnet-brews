using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brew.Features.Patterns.ChainOfResponsibility;

/// <summary>
/// Demonstrates the Chain of Responsibility Pattern - passing requests along a chain of handlers
/// Use Case: Purchase approval workflow with different authority levels
/// </summary>
public class ChainOfResponsibility : ModuleBase
{
    protected override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        services.AddSingleton<EntryLevel>();
        services.AddSingleton<MidLevel>();
        services.AddSingleton<Ceo>();
    }

    protected override async Task ExecuteAsync(CancellationToken token = default)
    {
        Logger.LogInformation("=== CHAIN OF RESPONSIBILITY PATTERN DEMONSTRATION ===\n");
        Logger.LogInformation("Scenario: Purchase approval system with hierarchical authority levels\n");

        // Get handlers from DI
        var teamLead = Host.Services.GetRequiredService<EntryLevel>();
        var manager = Host.Services.GetRequiredService<MidLevel>();
        var director = Host.Services.GetRequiredService<Ceo>();

        // Build the chain: Team Lead → Manager → Director
        Logger.LogInformation("╔════════════════════════════════════════════════════════════╗");
        Logger.LogInformation("║  Setting up Chain of Responsibility                       ║");
        Logger.LogInformation("╚════════════════════════════════════════════════════════════╝\n");
        
        teamLead.SetSuccessor(manager);
        manager.SetSuccessor(director);

        Logger.LogInformation("\nChain established: Team Lead → Manager → Director\n");

        // Create various purchase requests
        var requests = new[]
        {
            new Request 
            { 
                Type = RequestType.Small, 
                Amount = 500m, 
                Description = "Office supplies (printer paper, pens)" 
            },
            new Request 
            { 
                Type = RequestType.Small, 
                Amount = 850m, 
                Description = "Software licenses for team" 
            },
            new Request 
            { 
                Type = RequestType.Medium, 
                Amount = 5000m, 
                Description = "New development server" 
            },
            new Request 
            { 
                Type = RequestType.Medium, 
                Amount = 9500m, 
                Description = "Conference attendance for 5 team members" 
            },
            new Request 
            { 
                Type = RequestType.Large, 
                Amount = 25000m, 
                Description = "Enterprise software upgrade" 
            },
            new Request 
            { 
                Type = RequestType.Large, 
                Amount = 75000m, 
                Description = "New cloud infrastructure migration" 
            }
        };

        Logger.LogInformation("╔════════════════════════════════════════════════════════════╗");
        Logger.LogInformation("║  Processing Purchase Requests Through the Chain           ║");
        Logger.LogInformation("╚════════════════════════════════════════════════════════════╝");

        foreach (var request in requests)
        {
            Logger.LogInformation("\n" + new string('=', 60));
            Logger.LogInformation("🛒 NEW PURCHASE REQUEST:");
            Logger.LogInformation("   Description: {Description}", request.Description);
            Logger.LogInformation("   Amount: ${Amount:N2}", request.Amount);
            Logger.LogInformation("   Type: {Type}", request.Type);
            Logger.LogInformation(new string('=', 60));

            // Send request to start of chain - it will automatically flow through
            teamLead.ProcessRequest(request);

            // Show final result
            if (request.IsApproved)
            {
                Logger.LogInformation("\n✅ FINAL RESULT: Request APPROVED by {ApprovedBy}\n", request.ApprovedBy);
            }
            else
            {
                Logger.LogWarning("\n❌ FINAL RESULT: Request was NOT approved\n");
            }

            await Task.Delay(200); // Visual separation between requests
        }

        // Summary
        Logger.LogInformation("\n" + new string('═', 60));
        Logger.LogInformation("=== KEY BENEFITS OF CHAIN OF RESPONSIBILITY ===");
        Logger.LogInformation(new string('═', 60));
        Logger.LogInformation("✓ DECOUPLING: Sender doesn't need to know who will handle the request");
        Logger.LogInformation("✓ FLEXIBILITY: Easy to add/remove handlers or change order");
        Logger.LogInformation("✓ SINGLE RESPONSIBILITY: Each handler has one clear approval level");
        Logger.LogInformation("✓ AUTOMATIC ESCALATION: Requests flow up the chain until handled");
        Logger.LogInformation("✓ EXTENSIBILITY: New approval levels can be added without changing existing code");
        Logger.LogInformation("\nUse Cases: Approval workflows, event handling, logging systems, request processing pipelines");
    }
}