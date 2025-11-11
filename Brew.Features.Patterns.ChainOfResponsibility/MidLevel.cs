using Microsoft.Extensions.Logging;

namespace Brew.Features.Patterns.ChainOfResponsibility;

/// <summary>
/// Second handler in chain - Manager (can approve medium purchases $1,000-$10,000)
/// </summary>
public class MidLevel : Employee
{
    public MidLevel(ILogger<MidLevel> logger) : base(logger, "Manager")
    {
    }

    public override void ProcessRequest(Request request)
    {
        Logger.LogInformation("\n[{Title}] Received request: {Description} (${Amount:N2})", 
            Title, request.Description, request.Amount);

        if (request.Type == RequestType.Medium)
        {
            // This handler can process medium requests
            request.IsApproved = true;
            request.ApprovedBy = Title;
            Logger.LogInformation("[{Title}] ✓ APPROVED - Within my authority ($1,000 - $10,000)", Title);
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