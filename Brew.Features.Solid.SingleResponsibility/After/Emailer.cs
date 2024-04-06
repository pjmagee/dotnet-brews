using Microsoft.Extensions.Logging;

namespace Brew.Features.Solid.SingleResponsibility.After;

public class Emailer
{
    private readonly ILogger<Emailer> _logger;
    
    private string smtpAddress;
    private string fromAddress;

    public Emailer(ILogger<Emailer> logger, string address)  : this(address)
    {
        _logger = logger;
    }

    private Emailer(string smtpAddress)
    {
        this.smtpAddress = smtpAddress;
        fromAddress = "sales@example.com";
    }

    public void SendEmail(string emailTo, OrderAfter order)
    {
        // configure smtp details
        // build email subject from ORDER ID
        // email body contains order items and sum of payment
    }
}