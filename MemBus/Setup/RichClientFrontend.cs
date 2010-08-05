﻿using System.Threading.Tasks;
using MemBus.Messages;
using MemBus.Subscribing;

namespace MemBus
{
    /// <summary>
    /// Setup the Bus with this on the UI thread (Important!).
    /// A parallel publisher is used that publishes messages in parallel but blocks until all message processing is done.
    /// Exceptions will not stop the publishing and become available on the bus as <see cref="ExceptionOccurred"/> message.
    /// The setup allows you to call <see cref="ISubscriptionCustomizer{M}.DispatchOnUiThread"/> when doing a subscription.
    /// </summary>
    public class RichClientFrontend : IBusSetupConfigurator
    {
        public void Accept(IConfigurableBus setup)
        {
            setup.InsertPublishPipelineMember(new ParallelBlockingPublisher());
            setup.InsertResolver(new TableBasedResolver());
            setup.AddService(new SubscriptionMatroschkaFactory { new ShapeToDispose() });
            setup.AddService(TaskScheduler.FromCurrentSynchronizationContext());
        }
    }

    /// <summary>
    /// Setup the Bus with this on the UI thread (Important!).
    /// A parallel publisher is used that publishes messages in parallel. With this setup <see cref="IBus.Publish"/> does NOT block.
    /// Exceptions will become available once all subscriptions are done processing the message as <see cref="ExceptionOccurred"/> message.
    /// This setup allows you to call <see cref="ISubscriptionCustomizer{M}.DispatchOnUiThread"/> when doing a subscription.
    /// </summary>
    public class AsyncRichClientFrontend : IBusSetupConfigurator
    {
        public void Accept(IConfigurableBus setup)
        {
            setup.InsertPublishPipelineMember(new ParallelNonBlockingPublisher());
            setup.InsertResolver(new TableBasedResolver());
            setup.AddService(new SubscriptionMatroschkaFactory { new ShapeToDispose() });
            setup.AddService(TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}