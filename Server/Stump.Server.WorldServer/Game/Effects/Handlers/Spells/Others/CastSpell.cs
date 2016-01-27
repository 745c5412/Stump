﻿using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Others
{
    [EffectHandler(EffectsEnum.Effect_TriggerBuff)]
    [EffectHandler(EffectsEnum.Effect_TriggerBuff_793)]
    [EffectHandler(EffectsEnum.Effect_CastSpell_1160)]
    [EffectHandler(EffectsEnum.Effect_CastSpell_1017)]
    public class CastSpell : SpellEffectHandler
    {
        public CastSpell(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            foreach (var affectedActor in GetAffectedActors())
            {
                if (Dice.Duration != 0 || Dice.Delay != 0)
                {
                    var buffId = affectedActor.PopNextBuffId();

                    var spell = new Spell(Dice.DiceNum, (byte)Dice.DiceFace);
                    var effect = Effect as EffectDice;

                    var buff = new TriggerBuff(buffId, affectedActor, Caster, effect, spell, Spell, false, false, DefaultBuffTrigger)
                    {
                        Duration = (short)Dice.Duration,
                        Delay = (short)Dice.Delay
                    };

                    affectedActor.AddBuff(buff);
                }
                else
                    Caster.CastSpell(new Spell(Dice.DiceNum, (byte)Dice.DiceFace), affectedActor.Cell, true, true);
            }

            return true;
        }

        static void DefaultBuffTrigger(TriggerBuff buff, FightActor triggerrer, BuffTriggerType trigger, object token)
        {
            buff.Caster.CastSpell(buff.Spell, buff.Target.Cell, true, true);
        }
    }
}