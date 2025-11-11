namespace Brew.Features.Patterns.Adaptor;

/// <summary>
/// Modern payment request structure
/// </summary>
public class PaymentRequest
{
    public string CardNumber { get; set; } = string.Empty;
    public string ExpiryDate { get; set; } = string.Empty;
    public string Cvv { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
}

/// <summary>
/// Modern payment result structure
/// </summary>
public class PaymentResult
{
    public bool Success { get; set; }
    public string TransactionId { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

/// <summary>
/// Modern payment processor interface that our application expects
/// </summary>
public interface IModernPaymentProcessor
{
    PaymentResult ProcessPayment(PaymentRequest request);
    bool ValidateCard(string cardNumber);
    bool IsTransactionSuccessful(string transactionId);
}