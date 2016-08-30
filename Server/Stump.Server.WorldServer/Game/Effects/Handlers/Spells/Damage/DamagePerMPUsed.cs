﻿using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Spells;
using System;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Damage
{
    [EffectHandler(EffectsEnum.Effect_DamageAirPerMP)]
    [EffectHandler(EffectsEnum.Effect_DamageEarthPerMP)]
    [EffectHandler(EffectsEnum.Effect_DamageFirePerMP)]
    [EffectHandler(EffectsEnum.Effect_DamageWaterPerMP)]
    [EffectHandler(EffectsEnum.Effect_DamageNeutralPerMP)]
    public class DamagePerMPUsed : SpellEffectHandler
    {
        public DamagePerMPUsed(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            foreach (var actor in GetAffectedActors())
            {
                AddTriggerBuff(actor, true, BuffTriggerType.TURN_END, OnBuffTriggered);
            }

            return true;
        }

        private void OnBuffTriggered(TriggerBuff buff, BuffTriggerType trigger, object token)
        {
            var usedMP = buff.Target.UsedMP;
            if (usedMP <= 0)
                return;

            var damages = new Fights.Damage(Dice)
            {
                Source = buff.Caster,
                Buff = buff,
                School = GetEffectSchool(buff.Dice.EffectId),
                MarkTrigger = MarkTrigger,
                IsCritical = Critical,
                Spell = buff.Spell
            };

            damages.GenerateDamages();
            damages.Amount = usedMP * damages.BaseMaxDamages;

            buff.Target.InflictDamage(damages);
        }

        private static EffectSchoolEnum GetEffectSchool(EffectsEnum effect)
        {
            switch (effect)
            {
                case EffectsEnum.Effect_DamageWaterPerMP:
                    return EffectSchoolEnum.Water;

                case EffectsEnum.Effect_DamageEarthPerMP:
                    return EffectSchoolEnum.Earth;

                case EffectsEnum.Effect_DamageAirPerMP:
                    return EffectSchoolEnum.Air;

                case EffectsEnum.Effect_DamageFirePerMP:
                    return EffectSchoolEnum.Fire;

                case EffectsEnum.Effect_DamageNeutralPerMP:
                    return EffectSchoolEnum.Neutral;

                default:
                    throw new Exception(string.Format("Effect {0} has not associated School Type", effect));
            }
        }
    }
}