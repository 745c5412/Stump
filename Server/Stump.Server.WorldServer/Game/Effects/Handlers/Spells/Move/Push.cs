using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Handlers.Actions;
using Stump.Server.WorldServer.Game.Spells.Casts;
using System;
using Stump.DofusProtocol.Enums.Extensions;
using Stump.Server.WorldServer.AI.Fights.Spells;
using Stump.Server.WorldServer.Game.Maps.Cells;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Move
{
    [AISpellEffect(SpellCategory.Movement)]
    [EffectHandler(EffectsEnum.Effect_PushBack)]
    [EffectHandler(EffectsEnum.Effect_PushBack_1103)]
    [EffectHandler(EffectsEnum.Effect_PullForward)]
    public class Push : SpellEffectHandler
    {
        public Push(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
            : base(effect, caster, castHandler, targetedCell, critical)
        {
            DamagesDisabled = effect.EffectId == EffectsEnum.Effect_PushBack_1103 ||
                effect.EffectId == EffectsEnum.Effect_PullForward;
            Pull = effect.EffectId == EffectsEnum.Effect_PullForward;
        }

        public bool DamagesDisabled
        {
            get;
            set;
        }

        public DirectionsEnum? PushDirection
        {
            get;
            set;
        }

        public bool Pull
        {
            get;
            set;
        }

        public int Distance
        {
            get;
            set;
        }

        protected override bool InternalApply()
        {
            var integerEffect = GenerateEffect();

            if (integerEffect == null)
                return false;

            foreach (var actor in GetAffectedActors().OrderByDescending(entry => entry.Position.Point.ManhattanDistanceTo(TargetedPoint)))
            {
                if (!actor.CanBePushed() || actor.HasState((int)SpellStatesEnum.INEBRANLABLE_157))
                    continue;

                var referenceCell = TargetedCell.Id == actor.Cell.Id ? CastPoint : TargetedPoint;

                if (referenceCell.CellId == actor.Position.Cell.Id)
                    continue;

                if (PushDirection == null)
                    PushDirection = Pull ? actor.Position.Point.OrientationTo(referenceCell) : referenceCell.OrientationTo(actor.Position.Point);

                var startCell = actor.Position.Point;
                var lastCell = startCell;

                if (Distance == 0)
                    Distance = (short)(PushDirection.Value.IsDiagonal() ? Math.Ceiling(integerEffect.Value / 2.0) : integerEffect.Value);

                var stopCell = startCell.GetCellInDirection(PushDirection.Value, Distance);
                
                for (var i = 0; i < Distance; i++)
                {
                    var nextCell = lastCell.GetNearestCellInDirection(PushDirection.Value);

                    // the next cell is blocking, or an adjacent cell is blocking if it's in diagonal
                    if (IsBlockingCell(nextCell, actor) ||
                        (PushDirection.Value.IsDiagonal() && PushDirection.Value.GetDiagonalDecomposition().Any(x => IsBlockingCell(lastCell.GetNearestCellInDirection(x), actor))))
                    {
                        if (nextCell == null)
                        {
                            stopCell = lastCell;
                            nextCell = stopCell;
                        }

                        if (Fight.ShouldTriggerOnMove(Fight.Map.Cells[nextCell.CellId], actor))
                        {
                            DamagesDisabled = true;
                            stopCell = nextCell;
                        }
                        else
                            stopCell = lastCell;

                        break;
                    }

                    if (nextCell != null)
                        lastCell = nextCell;

                }

                if (actor.IsAlive())
                {
                    foreach (var character in Fight.GetCharactersAndSpectators().Where(actor.IsVisibleFor))
                        ActionsHandler.SendGameActionFightSlideMessage(character.Client, Caster, actor, startCell.CellId, stopCell.CellId);
                }

                if (!DamagesDisabled)
                {
                    var fightersInline = Fight.GetAllFightersInLine(startCell, Distance, PushDirection.Value);
                    fightersInline.Insert(0, actor);
                    var distance = integerEffect.Value - startCell.ManhattanDistanceTo(stopCell);
                    var targets = 0;

                    foreach (var fighter in fightersInline)
                    {
                        var pushDamages = Formulas.FightFormulas.CalculatePushBackDamages(Caster, fighter, (int)distance, targets);

                        if (pushDamages > 0)
                        {
                            var pushDamage = new Fights.Damage(pushDamages)
                            {
                                Source = actor,
                                School = EffectSchoolEnum.Pushback,
                                IgnoreDamageBoost = true,
                                IgnoreDamageReduction = false
                            };

                            fighter.InflictDamage(pushDamage);
                        }

                        targets++;
                    }             
                }

                if (actor.IsCarrying())
                    actor.ThrowActor(Map.Cells[startCell.CellId], true);

                actor.Position.Cell = Map.Cells[stopCell.CellId];

                if (Effect.EffectId != EffectsEnum.Effect_PullForward)
                    actor.TriggerBuffs(Caster, BuffTriggerType.OnPushed);
                actor.TriggerBuffs(Caster, BuffTriggerType.OnMoved);
            }

            return true;
        }

        private bool IsBlockingCell(MapPoint cell, FightActor target)
        {
            return cell == null || !Fight.IsCellFree(Map.Cells[cell.CellId]) || Fight.ShouldTriggerOnMove(Fight.Map.Cells[cell.CellId], target);
        }
    }
}