using Microsoft.Extensions.Logging;

namespace Brew.Feature.Events;

public class NativeEventReciever(ILogger<NativeEventReciever> logger, NativeEventNotifier notifier)
{
    public void Subscribe()
    {
        notifier.Notify += (sender, args) => logger.LogInformation("Native Event Recieved");
    }
}