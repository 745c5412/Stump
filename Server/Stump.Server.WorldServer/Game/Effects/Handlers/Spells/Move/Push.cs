using System.Linq;
using Stump.Core.Threading;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Handlers.Actions;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Move
{
    [EffectHandler(EffectsEnum.Effect_PushBack)]
    [EffectHandler(EffectsEnum.Effect_PushBack_1103)]
    public class Push : SpellEffectHandler
    {
        public Push(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }
        public bool DamagesDisabled
        {
            get;
            set;
        }

        public override bool Apply()
        {
            var integerEffect = GenerateEffect();

            if (integerEffect == null)
                return false;

            foreach (var actor in GetAffectedActors().OrderByDescending(entry => entry.Position.Point.DistanceToCell(TargetedPoint)))
            {
                if (actor.HasState((int)SpellStatesEnum.Unmovable))
                    continue;

                var referenceCell = TargetedCell.Id == actor.Cell.Id ? CastPoint : TargetedPoint;

                if (referenceCell.CellId == actor.Position.Cell.Id)
                    continue;

                var pushDirection = referenceCell.OrientationTo(actor.Position.Point, false);
                var startCell = actor.Position.Point;
                var lastCell = startCell;

                for (var i = 0; i < integerEffect.Value; i++)
                {
                    var nextCell = lastCell.GetNearestCellInDirection(pushDirection);

                    if (nextCell == null || !Fight.IsCellFree(Map.Cells[nextCell.CellId]))
                    {
                        var pushbackDamages = (8 + new AsyncRandom().Next(1, 8) * (Caster.Level / 50)) * (integerEffect.Value - i) + 
                            Caster.Stats[PlayerFields.PushDamageBonus] - actor.Stats[PlayerFields.PushDamageReduction];

                        if (!DamagesDisabled)
                        {
                            var damage = new Fights.Damage(pushbackDamages)
                            {
                                Source = Caster,
                                School = EffectSchoolEnum.Unknown,
                                IgnoreDamageBoost = true,
                                IgnoreDamageReduction = false
                            };

                            actor.InflictDamage(damage);
                        }

                        break;
                    }

                    if (Fight.ShouldTriggerOnMove(Fight.Map.Cells[nextCell.CellId], actor))
                    {
                        lastCell = nextCell;
                        break;
                    }

                    lastCell = nextCell;
                }

                var endCell = lastCell;
                var actorCopy = actor;

                foreach (var fighter in Fight.GetAllFighters<CharacterFighter>().Where(actorCopy.IsVisibleFor))
                    ActionsHandler.SendGameActionFightSlideMessage(fighter.Character.Client, Caster, actorCopy, startCell.CellId, endCell.CellId);

                actor.Position.Cell = Map.Cells[endCell.CellId];
            }

            return true;
        }
    }
}