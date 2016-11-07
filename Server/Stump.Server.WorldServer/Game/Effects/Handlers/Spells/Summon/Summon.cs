﻿using NLog;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.Fight.Customs;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Triggers;
using Stump.Server.WorldServer.Handlers.Actions;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Summon
{
    [EffectHandler(EffectsEnum.Effect_Summon)]
    public class Summon : SpellEffectHandler
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public Summon(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            var monster = MonsterManager.Instance.GetMonsterGrade(Dice.DiceNum, Dice.DiceFace);

            if (monster == null)
            {
                logger.Error("Cannot summon monster {0} grade {1} (not found)", Dice.DiceNum, Dice.DiceFace);
                return false;
            }

            if (monster.Template.UseSummonSlot && !Caster.CanSummon())
                return false;

            SummonedFighter summon;
            if (monster.Template.Id == 3287 || monster.Template.Id == 3288 || monster.Template.Id == 3289) //Turrets
                summon = new SummonedTurret(Fight.GetNextContextualId(), Caster, monster, Spell, TargetedCell);
            else if (monster.Template.Id == 285) //Coffre animé
            {
                summon = null;

                var slave = new LivingChest(Fight.GetNextContextualId(), Caster.Team, Caster, monster, TargetedCell);

                ActionsHandler.SendGameActionFightSummonMessage(Fight.Clients, slave);

                Caster.AddSlave(slave);
                Caster.Team.AddFighter(slave);

                Fight.TriggerMarks(slave.Cell, slave, TriggerType.MOVE);
            }
            else
            {
                if (Caster is CharacterFighter && monster.Template.CanPlay)
                {
                    summon = null;

                    var slave = new SlaveFighter(Fight.GetNextContextualId(), Caster.Team, Caster, monster, TargetedCell);

                    ActionsHandler.SendGameActionFightSummonMessage(Fight.Clients, slave);

                    Caster.AddSlave(slave);
                    Caster.Team.AddFighter(slave);

                    Fight.TriggerMarks(slave.Cell, slave, TriggerType.MOVE);
                }
                else
                {
                    summon = new SummonedMonster(Fight.GetNextContextualId(), Caster.Team, Caster, monster, TargetedCell);
                }
            }

            if (summon != null)
            {
                ActionsHandler.SendGameActionFightSummonMessage(Fight.Clients, summon);

                Caster.AddSummon(summon);
                Caster.Team.AddFighter(summon);

                Fight.TriggerMarks(summon.Cell, summon, TriggerType.MOVE);
            }

            return true;
        }
    }
}