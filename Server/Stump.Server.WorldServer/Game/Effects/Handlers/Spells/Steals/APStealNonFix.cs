﻿using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Handlers.Actions;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Steals
{
    [EffectHandler(EffectsEnum.Effect_StealAP_84)]
    public class APStealNonFix : SpellEffectHandler
    {
        public APStealNonFix(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
             : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            foreach (FightActor actor in GetAffectedActors())
            {
                var integerEffect = GenerateEffect();

                if (integerEffect == null)
                    return false;

                var value = 0;

                for (var i = 0; i < integerEffect.Value && value < actor.AP; i++)
                {
                    if (actor.RollAPLose(Caster, value))
                    {
                        value++;
                    }
                }

                var dodged = (short)(integerEffect.Value - value);

                if (dodged > 0)
                {
                    ActionsHandler.SendGameActionFightDodgePointLossMessage(Fight.Clients,
                        ActionsEnum.ACTION_FIGHT_SPELL_DODGED_PA, Caster, actor, dodged);
                }

                if (value <= 0)
                    return false;

                //AddStatBuff(actor, (short)(-value), PlayerFields.AP, true, (short)EffectsEnum.Effect_SubAP);
                actor.LostAP((short)value, Caster);
                actor.TriggerBuffs(BuffTriggerType.LOST_AP);

                if (Effect.Duration > 0)
                {
                    AddStatBuff(Caster, (short)(value), PlayerFields.AP, true, (short)EffectsEnum.Effect_AddAP_111);
                }
                else
                {
                    Caster.RegainAP((short)(value));
                }
            }

            return true;
        }
    }
}