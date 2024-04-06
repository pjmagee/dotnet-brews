using Microsoft.Extensions.Logging;

namespace Brew.Features.CSharp.Events;

public class NativeEventReceiver(ILogger<NativeEventReceiver> logger, NativeEventNotifier notifier)
{
    public void Subscribe()
    {
        notifier.Notify += (sender, args) => logger.LogInformation("Native Event Recieved");
    }
}