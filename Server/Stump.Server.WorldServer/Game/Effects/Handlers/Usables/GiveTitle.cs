﻿using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects.Handlers.Usables;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items.Player;

{
    [EffectHandler(EffectsEnum.Effect_AddTitle)]
    public class GiveUsableTitle : UsableEffectHandler
    {
        public GiveUsableTitle(EffectBase effect, Character target, BasePlayerItem item)
            : base(effect, target, item)
        {
        }

        public override bool Apply()
        {
            var integerEffect = Effect.GenerateEffect(EffectGenerationContext.Item) as EffectInteger;

            if (integerEffect == null)
                return false;

            UsedItems = NumberOfUses;
            Target.AddTitle(integerEffect.Value);

            return true;
        }
    }
}