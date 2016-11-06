using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Effects.Handlers.Items;
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

            if (Effects.Any(x => x.EffectId == EffectsEnum.Effect_AddRange || x.EffectId == EffectsEnum.Effect_AddRange_136))
            {
                if (dropOnItem.Effects.Exists(x => x.EffectId == EffectsEnum.Effect_AddRange || x.EffectId == EffectsEnum.Effect_AddRange_136))
                {
                    Owner.SendServerMessage("L'amélioration a échouée : L'objet possède déjà un PO.");
                    return false;
                }
            }
            else
            {
                if (dropOnItem.Effects.Exists(x => x.EffectId == EffectsEnum.Effect_AddMP
                    || x.EffectId == EffectsEnum.Effect_AddMP_128
                    || x.EffectId == EffectsEnum.Effect_AddAP_111))
                {
                    Owner.SendServerMessage("L'amélioration a échouée : L'objet possède déjà un PA, ou un PM.");
                    return false;
                }
            }

            ApplyEffects(dropOnItem, ItemEffectHandler.HandlerOperation.UNAPPLY);

            dropOnItem.Effects.AddRange(Effects);
            dropOnItem.Invalidate();

            ApplyEffects(dropOnItem, ItemEffectHandler.HandlerOperation.APPLY);
            Owner.Inventory.RefreshItem(dropOnItem);
            Owner.RefreshStats();

            Owner.SendServerMessage("Votre objet a été amélioré avec succès !");

            return true;
        }

        private void ApplyEffects(BasePlayerItem item, ItemEffectHandler.HandlerOperation operation)
        {
            foreach (var handler in item.Effects.Select(effect => EffectManager.Instance.GetItemEffectHandler(effect, Owner, this)))
            {
                handler.Operation = operation;

                if (Owner.Inventory.GetEquipedItems().Any(x => x != item && x.GetExoEffects().ToList().Exists(y => item.GetExoEffects().Any(z => z == y)))
                    && item.GetExoEffects().Any(x => x == handler.Effect))
                {
                    handler.Operation = ItemEffectHandler.HandlerOperation.NONAPPLY;
                }

                handler.Apply();
            }
        }
    }
}