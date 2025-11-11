namespace Brew.Features.Patterns.ChainOfResponsibility;

/// <summary>
/// Types of purchase requests based on amount thresholds
/// </summary>
public enum RequestType
{
    Small,    // Under $1,000 - Team Lead can approve
    Medium,   // $1,000 - $10,000 - Manager can approve  
    Large     // Over $10,000 - Director can approve
}