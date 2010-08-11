﻿using System;
using MemBus.Support;

namespace MemBus.Subscribing
{
    public class FilteredSubscription<M> : ISubscription, IDenyShaper
    {
        private readonly Func<M, bool> filter;
        private readonly ISubscription subscription;

        public FilteredSubscription(Func<M, bool> filter, ISubscription subscription)
        {
            this.filter = filter;
            this.subscription = subscription;
        }

        public void Push(object message)
        {
            if (filter((M) message))
                subscription.Push(message);
        }

        public Type Handles
        {
            get { return subscription.Handles; }
        }

        public bool Deny
        {
            get { return subscription.CheckDenyOrAllIsGood(); }
        }
    }
}