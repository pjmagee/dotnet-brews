using Microsoft.Extensions.Logging;

namespace Brew.Feature.Events;

public class NativeEventNotifier(ILogger<NativeEventNotifier> logger)
{
    public event EventHandler? Notify;

    public void Trigger()
    {
        logger.LogInformation("Native Event Triggered");
        Notify?.Invoke(this, EventArgs.Empty);
    }
}