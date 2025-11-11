namespace Brew.Features.Patterns.Adaptor;

/// <summary>
/// Legacy payment processor with incompatible interface
/// This represents a third-party or legacy system we cannot modify
/// </summary>
public class LegacyPaymentProcessor
{
    /// <summary>
    /// Legacy method with different parameter structure
    /// </summary>
    public string ProcessPaymentLegacy(string cardNum, string expiry, string cvv, decimal amt, string curr)
    {
        // Simulate legacy processing
        return $"LEGACY_TX_{Guid.NewGuid():N}";
    }

    /// <summary>
    /// Legacy method that returns status code instead of boolean
    /// </summary>
    public int ValidateCardLegacy(string cardNum)
    {
        // Legacy returns: 0 = invalid, 1 = valid, 2 = expired, 3 = fraud
        return cardNum.Length == 16 ? 1 : 0;
    }

    /// <summary>
    /// Legacy method with different naming convention
    /// </summary>
    public string GetTransactionStatusLegacy(string txId)
    {
        // Legacy returns status codes: "OK", "FAIL", "PEND"
        return "OK";
    }
}