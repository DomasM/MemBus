using System;
using System.Threading;

namespace MemBus.Tests.Help
{
    public class MockSubscription<T> : ISubscription
    {
        private readonly ManualResetEvent evtBlock;
        private readonly ManualResetEvent evtSignal;
        public int Received;

        public MockSubscription()
        {
        }

        public MockSubscription(ManualResetEvent evtBlock = null, ManualResetEvent evtSignal = null)
        {
            this.evtBlock = evtBlock;
            this.evtSignal = evtSignal;
        }

        public void Push(object message)
        {
            if (evtBlock != null)
                evtBlock.WaitOne();
            Received++;
            if (evtSignal != null)
              evtSignal.Set();
        }

        public Type Handles
        {
            get { return typeof(T); }
        }
    }
}