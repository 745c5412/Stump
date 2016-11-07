using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;
using System;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Damage
{
    [EffectHandler(EffectsEnum.Effect_DamageNeutralRemainingMP)]
    [EffectHandler(EffectsEnum.Effect_DamageAirRemainingMP)]
    [EffectHandler(EffectsEnum.Effect_DamageWaterRemainingMP)]
    [EffectHandler(EffectsEnum.Effect_DamageFireRemainingMP)]
    [EffectHandler(EffectsEnum.Effect_DamageEarthRemainingMP)]
    public class DamagePerRemainingMP : SpellEffectHandler
    {
        public DamagePerRemainingMP(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            foreach (var actor in GetAffectedActors())
            {
                var damages = new Fights.Damage(Dice)
                {
                    Source = Caster,
                    School = GetEffectSchool(Dice.EffectId),
                    MarkTrigger = MarkTrigger,
                    IsCritical = Critical
                };

                damages.BaseMaxDamages = (int) Math.Floor(damages.BaseMaxDamages*(Caster.Stats.MP.Total/(double) Caster.Stats.MP.TotalMax));
                damages.BaseMinDamages = (int) Math.Floor(damages.BaseMinDamages*(Caster.Stats.MP.Total/(double) Caster.Stats.MP.TotalMax));

                actor.InflictDamage(damages);
            }

            return true;
        }

        private static EffectSchoolEnum GetEffectSchool(EffectsEnum effect)
        {
            switch (effect)
            {
                case EffectsEnum.Effect_DamageWaterRemainingMP:
                    return EffectSchoolEnum.Water;

                case EffectsEnum.Effect_DamageEarthRemainingMP:
                    return EffectSchoolEnum.Earth;

                case EffectsEnum.Effect_DamageAirRemainingMP:
                    return EffectSchoolEnum.Air;

                case EffectsEnum.Effect_DamageFireRemainingMP:
                    return EffectSchoolEnum.Fire;

                case EffectsEnum.Effect_DamageNeutralRemainingMP:
                    return EffectSchoolEnum.Neutral;

                default:
                    throw new Exception(string.Format("Effect {0} has not associated School Type", effect));
            }
        }
    }
}