﻿using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using System;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Heal
{
    [EffectHandler(EffectsEnum.Effect_StealHPFix)]
    public class StealHpFix : SpellEffectHandler
    {
        public StealHpFix(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            foreach (var actor in GetAffectedActors())
            {
                var integerEffect = GenerateEffect();

                if (integerEffect == null)
                    return false;

                if (Effect.Duration > 0)
                {
                    AddTriggerBuff(actor, true, BuffTriggerType.TURN_BEGIN, OnBuffTriggered);
                }
                else
                {
                    var damages = new Fights.Damage(Dice, EffectSchoolEnum.Neutral, Caster, Spell)
                    {
                        IsCritical = Critical,
                        IgnoreDamageBoost = true,
                        IgnoreDamageReduction = false
                    };

                    damages.GenerateDamages();
                    var inflictedDamages = actor.InflictDamage(damages);

                    var heal = (int)Math.Floor(inflictedDamages / 2d);
                    Caster.Heal(heal, actor, false);
                }
            }

            return true;
        }

        private static void OnBuffTriggered(TriggerBuff buff, BuffTriggerType trigger, object token)
        {
            var damages = new Fights.Damage(buff.Dice, EffectSchoolEnum.Unknown, buff.Caster, buff.Spell)
            {
                Buff = buff,
                IsCritical = buff.Critical,
            };

            damages.GenerateDamages();
            buff.Target.InflictDirectDamage(damages.Amount);

            var heal = (int)Math.Floor(damages.Amount / 2d);
            buff.Caster.Heal(heal, buff.Target, false);
        }
    }
}