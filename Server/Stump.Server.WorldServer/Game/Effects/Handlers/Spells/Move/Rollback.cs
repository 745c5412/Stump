using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Handlers.Actions;
using System.Linq;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Move
{
    [EffectHandler(EffectsEnum.Effect_Rollback)]
    public class Rollback : SpellEffectHandler
    {
        public Rollback(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            var fighters = Fight.GetAllFighters(x => x.IsAlive() && !(x is SummonedFighter) && !(x is SummonedBomb));
            foreach (var fighter in fighters)
            {
                var newCell = fighter.FightStartPosition?.Cell;

                if (newCell == null)
                    continue;

                var oldFighter = Fight.GetOneFighter(newCell);
                if (oldFighter != null)
                    fighter.ExchangePositions(oldFighter);
                else
                {
                    fighter.Position.Cell = newCell;
                    ActionsHandler.SendGameActionFightTeleportOnSameMapMessage(Fight.Clients, Caster, fighter, newCell);
                }
            }

            return true;
        }
    }
}