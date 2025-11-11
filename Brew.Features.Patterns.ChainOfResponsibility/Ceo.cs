using Microsoft.Extensions.Logging;

namespace Brew.Features.Patterns.ChainOfResponsibility;

/// <summary>
/// Final handler in chain - Director (can approve large purchases over $10,000)
/// </summary>
public class Ceo : Employee
{
    public Ceo(ILogger<Ceo> logger) : base(logger, "Director")
    {
    }

    public override void ProcessRequest(Request request)
    {
        Logger.LogInformation("\n[{Title}] Received request: {Description} (${Amount:N2})", 
            Title, request.Description, request.Amount);

        if (request.Type == RequestType.Large)
        {
            // Final handler processes large requests
            request.IsApproved = true;
            request.ApprovedBy = Title;
            Logger.LogInformation("[{Title}] ✓ APPROVED - I have final authority for large purchases (> $10,000)", Title);
        }
        else
        {
            // This should not happen if chain is set up correctly
            Logger.LogWarning("[{Title}] ⚠ Unexpected request type - no further handlers in chain", Title);
        }
    }
}