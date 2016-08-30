﻿using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Buffs
{
    [EffectHandler(EffectsEnum.Effect_AddComboBonus)]
    public class ComboBonusBuff : SpellEffectHandler
    {
        public ComboBonusBuff(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            foreach (var actor in GetAffectedActors())
            {
                var bomb = actor as SummonedBomb;
                if (bomb == null)
                    continue;

                var integerEffect = GenerateEffect();

                if (integerEffect == null)
                    return false;

                if (Effect.Duration > 1)
                    AddStatBuff(actor, integerEffect.Value, PlayerFields.ComboBonus, false);
                else
                    bomb.IncreaseDamageBonus(integerEffect.Value);
            }

            return true;
        }
    }
}