﻿using System;
using MemBus.Subscribing;

namespace MemBus.Tests.Help
{
    public class SomeHandler : IAcceptDisposeToken
    {
        public int MsgACalls;
        public int MsgBCalls;

        public readonly MessageC MsgC = new MessageC();

        private IDisposable _disposeToken;

        public void Handle(MessageA msg)
        {
            MsgACalls++;
        }

        public MessageC Route(MessageB msg)
        {
            MsgBCalls++;
            return MsgC;
        }

        public void InvokeDisposeToken()
        {
            if (_disposeToken != null)
                _disposeToken.Dispose();
        }

        public void Accept(IDisposable disposeToken)
        {
            _disposeToken = disposeToken;
        }
    }
}