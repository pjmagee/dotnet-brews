using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Brew.Features;

public class Events : IBrew
{
    private readonly ILogger<Events> logger;

    public Events(ILogger<Events> logger)
    {
        this.logger = logger;
    }

    public void Before()
    {
        NativeEventNotifier notifier = new NativeEventNotifier(new Logger<NativeEventNotifier>(new NullLoggerFactory()));
        notifier.Notify += (sender, args) => logger.LogInformation("C# Event handler called.");
        notifier.Trigger();
    }

    public void After()
    {
        ServiceCollection services = new ServiceCollection();
        services.AddMediatR(typeof(Events));
        services.AddSingleton(typeof(ILogger), logger);

        Task.WaitAll(Task.Run(async () =>
        {
            await using (var provider = services.BuildServiceProvider())
            {
                var mediator = provider.GetRequiredService<IMediator>();
                await mediator.Publish(new MediatorNotification(), CancellationToken.None);
            }
        }));
    }

    public class MediatorNotification : INotification
    {

    }

    public class MediatorNotifier : INotificationHandler<MediatorNotification>
    {
        private readonly ILogger logger;

        public MediatorNotifier(ILogger logger)
        {
            this.logger = logger;
        }

        public async Task Handle(MediatorNotification notification, CancellationToken cancellationToken)
        {
            logger.LogInformation("Mediator Event Handler called.");
        }
    }

    public class NativeEventNotifier
    {
        private readonly ILogger logger;
        public event EventHandler Notify;

        public NativeEventNotifier(ILogger<NativeEventNotifier> logger) => this.logger = logger;

        public void Trigger() => Notify?.Invoke(this, EventArgs.Empty);
    }
}