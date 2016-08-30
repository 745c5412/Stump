using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;
using System.Collections.Generic;

namespace Stump.Server.WorldServer.Handlers.Context
{
    public partial class ContextHandler : WorldHandlerContainer
    {
        public static void SendNotificationListMessage(IPacketReceiver client, IEnumerable<int> notifications)
        {
            client.Send(new NotificationListMessage(notifications));
        }
    }
}