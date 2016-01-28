﻿using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.TaxCollectors;
using Stump.Server.WorldServer.Game.Effects.Handlers.Usables;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items.Player;
using Stump.Server.WorldServer.Game.Spells.Casts;
namespace Stump.Server.WorldServer.Game.Effects.Handlers.Items
{
    [EffectHandler(EffectsEnum.Effect_SummonTaxcollector)]
    public class SummonTaxcollector : UsableEffectHandler
    {
        public SummonTaxcollector(EffectBase effect, Character target, BasePlayerItem item)
            : base(effect, target, item)
        {
        }

        public override bool Apply()
        {
            if (Target.Guild == null)
                return false;

            if (!TaxCollectorManager.Instance.AddTaxCollectorSpawn(Target))
                return false;

            UsedItems = 1;
            return true;
        }
    }
}
