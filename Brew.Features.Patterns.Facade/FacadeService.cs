using Microsoft.Extensions.Logging;

namespace Brew.Features.Patterns.Facade;

/// <summary>
/// Facade - Provides a simplified interface to the complex subsystem
/// Hides the complexity of coordinating multiple services
/// </summary>
public class FacadeService(
    ComplexServiceC auditService,
    ComplexServiceB authService,
    ComplexServiceA databaseService,
    ILogger<FacadeService> logger)
{
    /// <summary>
    /// Simple method that hides all the complexity of the subsystem
    /// </summary>
    public void GenerateReport()
    {
        logger.LogInformation("\n[FACADE] Starting report generation with single method call...\n");

        // The facade coordinates all the complex subsystem interactions
        // Client doesn't need to know the order or details
        auditService.InitializeAudit();
        authService.AuthenticateUser();
        authService.AuthorizeOperation();
        databaseService.ValidateDatabase();
        databaseService.QueryData();
        auditService.LogOperation("Report Generation");
        databaseService.CloseConnection();
        authService.Logout();
        auditService.FinalizeAudit();

        logger.LogInformation("\n[FACADE] ✓ Report generated successfully!\n");
    }
}