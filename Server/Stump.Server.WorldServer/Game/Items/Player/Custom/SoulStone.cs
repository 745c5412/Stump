﻿using System.Linq;
using Stump.Core.Mathematics;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights;

namespace Stump.Server.WorldServer.Game.Items.Player.Custom
{
    [ItemType(ItemTypeEnum.SOUL_STONE)]
    public sealed class SoulStone : BasePlayerItem
    {
        private EffectDice m_soulStoneEffect;

        public SoulStone(Character owner, PlayerItemRecord record)
            : base(owner, record)
        {
            m_soulStoneEffect = Effects.OfType<EffectDice>().FirstOrDefault(x => x.EffectId == EffectsEnum.Effect_SoulStone);

            if (m_soulStoneEffect == null)
            {
                return;
            }

            if (IsEquiped())
                SubscribeEvents();

        }

        private void SubscribeEvents()
        {
            Owner.ContextChanged += OnContextChanged;
        }

        private void UnsubscribeEvents()
        {
            Owner.ContextChanged -= OnContextChanged;
        }

        private void OnContextChanged(Character character, bool infight)
        {
            if (infight)
                character.Fight.GeneratingResults += OnGeneratingResults;
        }

        private void OnGeneratingResults(IFight obj)
        {
            if (Owner.Fighter.Fight is FightPvM fightPvM && fightPvM == obj && !fightPvM.IsPvMArenaFight && Owner.Fighter.HasWin() && Owner.Fighter.HasState((int)SpellStatesEnum.Soul_Seeker))
            {
                if (Owner.Fighter.Team.Fighters.Any(x => x.Loot.Items.Any(y => y.Key == (int)ItemIdEnum.FullSoulStone)))
                    return;

                var highestLevel = Owner.Fighter.OpposedTeam.Fighters.Max(x => x.Level);

                if (highestLevel <= Power)
                {
                    var rand = new CryptoRandom();

                    if (rand.NextDouble() * 100 <= Probability)
                    {
                        if (Owner.Inventory.RemoveItem(this, 1) <= 0)
                            return;

                        var fullStone = ItemManager.Instance.CreatePlayerItem(Owner, (int)ItemIdEnum.FullSoulStone, 1) as SoulStoneFilled;

                        if (fullStone == null)
                            return;

                        fullStone.SetMonsterGroup(fightPvM.DefendersTeam.Fighters.OfType<MonsterFighter>().Select(x => x.Monster));

                        Owner.Inventory.AddItem(fullStone, false);
                        // display purpose
                        Owner.Fighter.Loot.AddItem(new DroppedItem(fullStone.Template.Id, 1) { IgnoreGeneration = true });
                    }
                }
            }
        }

        public override bool OnEquipItem(bool unequip)
        {
            if (!unequip)
                SubscribeEvents();
            else
                UnsubscribeEvents();

            return base.OnEquipItem(unequip);
        }

        public override bool OnRemoveItem()
        {
            UnsubscribeEvents();
            return base.OnRemoveItem();
        }

        public int? Probability => m_soulStoneEffect?.DiceNum;
        public int? Power => m_soulStoneEffect?.Value;
    }
}