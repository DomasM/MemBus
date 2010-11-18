using System;
using MemBus.Subscribing;
using MemBus.Tests.Help;
using Moq;
using NUnit.Framework;
using MemBus.Tests.Frame;

namespace MemBus.Tests
{
    [TestFixture]
    public class Using_disposable_method_subscription
    {
        [Test]
        public void the_basic_method_subscription_is_contravariant()
        {
            var sub = new MethodInvocation<MessageA>(msg => { });
            sub.Handles(typeof(MessageASpecialization)).ShouldBeTrue();
        }

        [Test]
        public void An_implementator_of_handles_acts_contravariant()
        {
            var mock = new Mock<Handles<MessageA>>();
            ((IHandles<MessageA>)mock.Object).Handles(typeof(MessageASpecialization)).ShouldBeTrue();
        }

        
        [Test]
        public void handle_type_derived_from_Action()
        {
            int callCount = 0;
            var sub = new DisposableSubscription(new MethodInvocation<MessageA>(msg => callCount++));
            sub.Handles(typeof(MessageA)).ShouldBeTrue();
        }

        [Test]
        public void passed_in_action_is_called()
        {
            int callCount = 0;
            var sub = new DisposableSubscription(new MethodInvocation<MessageA>(msg => callCount++));
            sub.Push(new MessageA());
            callCount.ShouldBeEqualTo(1);
        }

        [Test]
        public void calling_disposer_raises_disposed_evt()
        {
            var disposeCalled = false;
            IDisposableSubscription sub = new DisposableSubscription(null);
            sub.Disposed += (s, e) => disposeCalled = true;
            sub.GetDisposer().Dispose();
            disposeCalled.ShouldBeTrue();
            
        }
    }
}