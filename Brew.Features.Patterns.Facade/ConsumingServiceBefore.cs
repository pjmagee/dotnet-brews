using Microsoft.Extensions.Logging;

namespace Brew.Features.Patterns.Facade;

/// <summary>
/// BEFORE Facade - Client must understand and coordinate ALL subsystem components
/// This is complex, error-prone, and tightly coupled to the subsystem
/// </summary>
public class ConsumingServiceBefore(
    ComplexServiceC auditService,
    ComplexServiceB authService,
    ComplexServiceA databaseService,
    ILogger<ConsumingServiceBefore> logger)
{
    public void GenerateReport()
    {
        logger.LogInformation("\n[CLIENT] Having to manually coordinate all subsystems...\n");

        // Client must know:
        // 1. All the services that exist
        // 2. The correct order to call them
        // 3. Which methods to call on each service
        // 4. How they interact with each other
        // This is COMPLEX and ERROR-PRONE!

        auditService.InitializeAudit();
        authService.AuthenticateUser();
        authService.AuthorizeOperation();
        databaseService.ValidateDatabase();
        databaseService.QueryData();
        auditService.LogOperation("Report Generation");
        databaseService.CloseConnection();
        authService.Logout();
        auditService.FinalizeAudit();

        logger.LogInformation("\n[CLIENT] Report generated (but with lots of complexity!)\n");
    }
}