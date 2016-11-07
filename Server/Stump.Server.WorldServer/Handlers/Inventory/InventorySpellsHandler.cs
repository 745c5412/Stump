using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Core.Network;
using System.Linq;

namespace Stump.Server.WorldServer.Handlers.Inventory
{
    public partial class InventoryHandler
    {
        public static void SendSpellListMessage(WorldClient client, bool previsualization)
        {
            client.Send(new SpellListMessage(previsualization,
                                             client.Character.Spells.GetSpells().Select(
                                                 entry => entry.GetSpellItem())));
        }
    }
}