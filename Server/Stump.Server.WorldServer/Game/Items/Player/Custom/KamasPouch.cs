using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Items.Player.Custom
{
    [ItemType(ItemTypeEnum.KAMAS_POUCH)]
    public sealed class KamasPouch : BasePlayerItem
    {
        public KamasPouch(Character owner, PlayerItemRecord record)
            : base(owner, record)
        {
        }

        public override uint UseItem(int amount = 1, Cell targetCell = null, Character target = null)
        {
            var wonKamas = (int)(Template.Price * amount);

            Owner.Inventory.AddKamas(wonKamas);
            Owner.SendServerMessage(string.Format("Vous avez reçu {0} Kamas en utilisant votre {1}", wonKamas, Template.Name));

            return (uint)amount;
        }
    }
}