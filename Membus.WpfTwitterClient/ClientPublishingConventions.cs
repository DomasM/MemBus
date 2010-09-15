using MemBus.Publishing;
using MemBus.Setup;
using MemBus.Subscribing;
using MemBus.Support;
using Membus.Tests.WpfClient.Messages;

namespace Membus.Tests.WpfClient
{
    public class ClientPublishingConventions : ISetup<IConfigurableBus>
    {
        public void Accept(IConfigurableBus setup)
        {
            setup.ConfigurePublishing(
                    p =>
                    {
                        p.MessageMatch(
                            m => m.IsType<Transport>(),
                            l => l.PublishPipeline(new SequentialPublisher())
                            );
                        p.MessageMatch(
                            m => m.Name.EndsWith("Request"),
                            l => l.PublishPipeline(Publish.This(new Transport { On = true }), new ParallelNonBlockingPublisher())
                            );
                        p.MessageMatch(
                            m => m.Name.EndsWith("Response"),
                            l => l.PublishPipeline(new ParallelBlockingPublisher(), Publish.This(new Transport { On = false }))
                            );
                    });
            setup.ConfigureSubscribing(s => s.MessageMatch(
                m => m.Name.EndsWith("Response") || m.IsType<Transport>(),
                c => c.ShapeOutwards(new ShapeToUiDispatch(), new ShapeToDispose())));
        }
    }
}