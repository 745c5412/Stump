using Stump.Server.BaseServer.Handler;
using System;

namespace FakeClients.Handlers
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class FakeHandlerAttribute : HandlerAttribute
    {
        public FakeHandlerAttribute(uint messageId)
            : base(messageId)
        {
        }
    }
}