﻿using System.Collections.Generic;
using MemBus.Configurators;
using MemBus.Subscribing;
using NUnit.Framework;

namespace MemBus.Tests.Subscribing
{
    public class MessageReceiver
    {
        public readonly List<object> Messages = new List<object>();

        public void Clear()
        {
            Messages.Clear();
        }

        public void Add(object msg)
        {
            Messages.Add(msg);
        }
    }

    public class FlexibleSubscribingIntegrationContext
    {
        protected IBus Bus;
        protected List<object> Messages = new List<object>();

        [TestFixtureSetUp]
        public void Given()
        {
            Bus = BusSetup
                .StartWith<Conservative>()
                .Apply<FlexibleSubscribeAdapter>(ConfigureAdapter)
                .Construct();
            Bus.Subscribe<object>(MessageCapturing);
            foreach (var endpoints in GetEndpoints())
            {
                Bus.Subscribe(endpoints);
            }
            AdditionalSetup();
        }

        protected virtual void AdditionalSetup()
        {
            
        }

        protected virtual IEnumerable<object> GetEndpoints()
        {
            yield break;
        }

        private void MessageCapturing(object message)
        {
            Messages.Add(message);
        }

        protected virtual void ConfigureAdapter(FlexibleSubscribeAdapter adp)
        {
        }

        protected void Clear()
        {
            Messages.Clear();
        }
    }
}