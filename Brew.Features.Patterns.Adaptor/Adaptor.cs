using Microsoft.Extensions.Logging;

namespace Brew.Features.Patterns.Adaptor;

/// <summary>
/// Adapter that makes the legacy payment processor compatible with modern interface
/// This is the key pattern: wrapping an incompatible interface to make it compatible
/// </summary>
public class PaymentProcessorAdapter : IModernPaymentProcessor
{
    private readonly LegacyPaymentProcessor _legacyProcessor;
    private readonly ILogger<PaymentProcessorAdapter> _logger;

    public PaymentProcessorAdapter(LegacyPaymentProcessor legacyProcessor, ILogger<PaymentProcessorAdapter> logger)
    {
        _legacyProcessor = legacyProcessor;
        _logger = logger;
    }

    /// <summary>
    /// Adapts modern PaymentRequest to legacy method signature
    /// </summary>
    public PaymentResult ProcessPayment(PaymentRequest request)
    {
        _logger.LogInformation("[Adapter] Converting modern PaymentRequest to legacy format...");
        _logger.LogInformation("[Adapter] Modern format: PaymentRequest object with properties");
        _logger.LogInformation("[Adapter] Legacy format: Individual parameters (cardNum, expiry, cvv, amt, curr)");

        // Adapt: Convert modern object to legacy parameters
        var transactionId = _legacyProcessor.ProcessPaymentLegacy(
            request.CardNumber,
            request.ExpiryDate,
            request.Cvv,
            request.Amount,
            request.Currency
        );

        _logger.LogInformation("[Adapter] Legacy processor returned transaction ID: {TransactionId}", transactionId);

        // Adapt: Convert legacy response to modern format
        return new PaymentResult
        {
            Success = true,
            TransactionId = transactionId,
            Message = "Payment processed successfully via legacy system"
        };
    }

    /// <summary>
    /// Adapts legacy status code (int) to modern boolean
    /// </summary>
    public bool ValidateCard(string cardNumber)
    {
        _logger.LogInformation("[Adapter] Converting legacy validation (int status code) to modern format (bool)...");

        // Call legacy method that returns int
        var statusCode = _legacyProcessor.ValidateCardLegacy(cardNumber);

        // Adapt: Convert legacy status code to boolean
        var isValid = statusCode == 1;

        _logger.LogInformation("[Adapter] Legacy returned status code: {StatusCode}, converted to: {IsValid}", 
            statusCode, isValid);

        return isValid;
    }

    /// <summary>
    /// Adapts legacy status string ("OK"/"FAIL") to modern boolean
    /// </summary>
    public bool IsTransactionSuccessful(string transactionId)
    {
        _logger.LogInformation("[Adapter] Converting legacy status string to modern boolean...");

        // Call legacy method that returns status string
        var status = _legacyProcessor.GetTransactionStatusLegacy(transactionId);

        // Adapt: Convert legacy status string to boolean
        var isSuccessful = status == "OK";

        _logger.LogInformation("[Adapter] Legacy returned: '{Status}', converted to: {IsSuccessful}", 
            status, isSuccessful);

        return isSuccessful;
    }
}