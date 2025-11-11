using Microsoft.Extensions.Logging;

namespace Brew.Features.Patterns.Facade;

/// <summary>
/// AFTER Facade - Client only needs the Facade, which provides a simple interface
/// Clean, simple, and loosely coupled
/// </summary>
public class ConsumingServiceAfter(FacadeService facadeService, ILogger<ConsumingServiceAfter> logger)
{
    public void GenerateReport()
    {
        logger.LogInformation("\n[CLIENT] Using Facade - just one simple call!\n");

        // Simple! The facade handles all the complexity
        // Client doesn't need to know about:
        // - Individual subsystem services
        // - The order of operations
        // - How services interact
        facadeService.GenerateReport();

        logger.LogInformation("\n[CLIENT] Done! Much simpler with Facade!\n");
    }
}