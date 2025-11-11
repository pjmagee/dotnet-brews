using Microsoft.Extensions.Logging;

namespace Brew.Features.Patterns.ChainOfResponsibility;

/// <summary>
/// First handler in chain - Team Lead (can approve small purchases under $1,000)
/// </summary>
public class EntryLevel : Employee
{
    public EntryLevel(ILogger<EntryLevel> logger) : base(logger, "Team Lead")
    {
    }

    public override void ProcessRequest(Request request)
    {
        Logger.LogInformation("\n[{Title}] Received request: {Description} (${Amount:N2})", 
            Title, request.Description, request.Amount);

        if (request.Type == RequestType.Small)
        {
            // This handler can process small requests
            request.IsApproved = true;
            request.ApprovedBy = Title;
            Logger.LogInformation("[{Title}] ✓ APPROVED - Within my authority (< $1,000)", Title);
        }
        else
        {
            // Pass to next handler in chain
            Logger.LogInformation("[{Title}] ⚠ Cannot approve - Amount exceeds my authority, forwarding to {SuccessorTitle}...", 
                Title, Successor?.Title ?? "No one");
            Successor?.ProcessRequest(request);
        }
    }
}