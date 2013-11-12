﻿using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using MemBus.Subscribing;
using MemBus.Support;
using System.Linq;

namespace MemBus
{
    internal class CompositeSubscription : ISubscription, IEnumerable<ISubscription>
    {
        private readonly ConcurrentDictionary<int,IDisposableSubscription> _subscriptions = new ConcurrentDictionary<int,IDisposableSubscription>();

        public CompositeSubscription() { }

        public CompositeSubscription(IEnumerable<ISubscription> subscriptions)
        {
            AddRange(subscriptions);
        }

        public bool IsEmpty
        {
            get { return _subscriptions.IsEmpty; }
        }

        public void Push(object message)
        {
            foreach (var s in _subscriptions.Values)
                s.Push(message);
        }

        bool ISubscription.Handles(Type messageType)
        {
            return _subscriptions.Values.All(s => s.Handles(messageType));
        }

        public event EventHandler Disposed;

        public void Add(ISubscription subscription)
        {
            if (subscription == null)
                throw new ArgumentNullException("subscription", "Attempt to add a Null Reference to Composite subscription.");
            IDisposableSubscription disposableSub = getDisposableSub(subscription);
            disposableSub.Disposed += onSubscriptionDisposed;
            _subscriptions.AddOrUpdate(disposableSub.GetHashCode(), _ => disposableSub, (_,__) => disposableSub);
        }

        private static IDisposableSubscription getDisposableSub(ISubscription subscription)
        {
            return subscription is IDisposableSubscription ? 
                   (IDisposableSubscription)subscription : new DisposableSubscription(subscription);
        }

        private void onSubscriptionDisposed(object sender, EventArgs e)
        {
            IDisposableSubscription value;
            _subscriptions.TryRemove(sender.GetHashCode(), out value);
            Disposed.Raise(sender);
        }

        public IEnumerator<ISubscription> GetEnumerator()
        {
            return _subscriptions.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void AddRange(IEnumerable<ISubscription> subscriptions)
        {
            foreach (var s in subscriptions)
                Add(s);
        }

    }
}