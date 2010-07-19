using Moq;
using NUnit.Framework;
using MemBus.Tests.Frame;

namespace MemBus.Tests
{
    [TestFixture]
    public class When_using_the_bus
    {
        [Test]
        public void Default_setup_routes_the_message_correctly()
        {
            var sub = new Mock<ISubscription>();
            sub.Setup(s => s.Handles).Returns(typeof (MessageA));
            var b = BusSetup
                .StartWith<DefaultSetup>(new AdHocConfigurator(cb => cb.AddSubscription(sub.Object)))
                .Construct();
            var messageA = new MessageA();
            b.Publish(messageA);
            sub.Verify(s=>s.Push(messageA));
        }

        [Test]
        public void Default_setup_provides_subscription_shape()
        {
            var received = 0;
            var b = BusSetup.StartWith<DefaultSetup>().Construct();
            var d = b.Subscribe<MessageA>(msg => received++);
            d.Dispose();
            received.ShouldBeEqualTo(1);
        }
        
    }
}