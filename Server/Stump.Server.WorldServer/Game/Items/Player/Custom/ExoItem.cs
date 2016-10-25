﻿using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using System.Linq;

namespace Stump.Server.WorldServer.Game.Items.Player.Custom
{
    [ItemType(ItemTypeEnum.EXO_POTION)]
    public class ExoItem : BasePlayerItem
    {
        public ExoItem(Character owner, PlayerItemRecord record)
            : base(owner, record)
        {
        }

        public override bool AllowDropping => true;

        public override bool Drop(BasePlayerItem dropOnItem)
        {
            var allowedItemType = new[] {
                ItemTypeEnum.AMULET,
                ItemTypeEnum.BOW,
                ItemTypeEnum.WAND,
                ItemTypeEnum.STAFF,
                ItemTypeEnum.DAGGER,
                ItemTypeEnum.SWORD,
                ItemTypeEnum.HAMMER,
                ItemTypeEnum.SHOVEL,
                ItemTypeEnum.RING,
                ItemTypeEnum.BELT,
                ItemTypeEnum.BOOTS,
                ItemTypeEnum.HAT,
                ItemTypeEnum.CLOAK,
                ItemTypeEnum.AXE,
                ItemTypeEnum.PICKAXE,
                ItemTypeEnum.SCYTHE,
                ItemTypeEnum.BACKPACK
            };

            if (!allowedItemType.Contains((ItemTypeEnum)dropOnItem.Template.TypeId))
            {
                Owner.SendServerMessage("L'amélioration a échouée : Vous ne pouvez pas améliorer ce type d'objet.");
                return false;
            }

            if (dropOnItem.Effects.Exists(x => x.EffectId == EffectsEnum.Effect_AddMP || x.EffectId == EffectsEnum.Effect_AddMP_128 || x.EffectId == EffectsEnum.Effect_AddAP_111))
            {
                Owner.SendServerMessage("L'amélioration a échouée : L'objet possède déjà un PA ou un PM.");
                return false;
            }

            dropOnItem.Effects.AddRange(Effects);
            Owner.Inventory.RefreshItem(dropOnItem);
            Owner.Inventory.ApplyItemEffects(dropOnItem);

            Owner.SendServerMessage("Votre objet a été amélioré avec succès !");

            return true;
        }
    }
}