using Stump.Core.Pool;
using System.Net.Sockets;

namespace Stump.Server.BaseServer.Network
{
    public class PoolableSocketArgs : SocketAsyncEventArgs, IPooledObject
    {
        public void Cleanup()
        {
            UserToken = null;
        }
    }
}