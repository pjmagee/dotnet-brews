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
        notifier.Notify += (sender, args) => logger.LogInformation("C# Event handler called");
        notifier.Trigger();
    }

    public void After()
    {
        ServiceCollection services = new ServiceCollection();

        services.AddMediatR(x =>
        {
            x.RegisterServicesFromAssemblyContaining<Events>();
        })
        .AddSingleton(typeof(ILogger), logger);

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

    public class MediatorNotifier(ILogger logger) : INotificationHandler<MediatorNotification>
    {
        public Task Handle(MediatorNotification notification, CancellationToken cancellationToken)
        {
            logger.LogInformation("Mediator Event Handler called");
            
            return Task.CompletedTask;
        }
    }

    public class NativeEventNotifier(ILogger<NativeEventNotifier> logger)
    {
        public event EventHandler? Notify;

        public void Trigger()
        {
            logger.LogInformation("Native Event Triggered");
            Notify?.Invoke(this, EventArgs.Empty);
        }
    }
}