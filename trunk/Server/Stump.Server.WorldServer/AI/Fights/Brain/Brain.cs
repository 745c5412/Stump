using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NLog;
using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.AI.Fights.Actions;
using Stump.Server.WorldServer.AI.Fights.Spells;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Maps.Pathfinding;
using Stump.Server.WorldServer.Game.Spells;
using TreeSharp;

namespace Stump.Server.WorldServer.AI.Fights.Brain
{
    public class Brain
    {
        public const int MaxMovesTries = 20;
        public const int MaxCastLimit = 20;


        [Variable(true)]
        public static bool DebugMode = false;

        protected static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public Brain(AIFighter fighter)
        {
            Fighter = fighter;
            Environment = new EnvironmentAnalyser(Fighter);
            SpellSelector = new SpellSelector(Fighter, Environment);
        }

        public AIFighter Fighter
        {
            get;
            private set;
        }

        public SpellSelector SpellSelector
        {
            get;
            private set;
        }

        public EnvironmentAnalyser Environment
        {
            get;
            private set;
        }

        public virtual void Play()
        {
            SpellSelector.AnalysePossibilities();
            foreach (var cast in SpellSelector.EnumerateSpellsCast())
            {
                if (cast.MoveBefore != null)
                {
                    Fighter.Fight.StartSequence(SequenceTypeEnum.SEQUENCE_MOVE);
                    var success = Fighter.StartMove(cast.MoveBefore);
                    var lastPos = Fighter.Cell.Id;

                    var tries = 0;
                    var destinationId = cast.MoveBefore.EndCell.Id;
                    // re-attempt to move if we didn't reach the cell i.e as we trigger a trap
                    while (success && Fighter.Cell.Id != destinationId && Fighter.CanMove() && tries <= MaxMovesTries)
                    {
                        var pathfinder = new Pathfinder(Environment.CellInformationProvider);
                        var path = pathfinder.FindPath(Fighter.Position.Cell.Id, destinationId, false, Fighter.MP);

                        if (path == null || path.IsEmpty())
                        {
                            Fighter.Fight.EndSequence(SequenceTypeEnum.SEQUENCE_MOVE);
                            break;
                        }

                        if (path.MPCost > Fighter.MP)
                        {
                            Fighter.Fight.EndSequence(SequenceTypeEnum.SEQUENCE_MOVE);
                            break;
                        }

                        success = Fighter.StartMove(path);

                        // the mob didn't move so we give up
                        if (Fighter.Cell.Id == lastPos)
                        {
                            Fighter.Fight.EndSequence(SequenceTypeEnum.SEQUENCE_MOVE);
                            break;
                        }

                        lastPos = Fighter.Cell.Id;
                        tries++; // avoid infinite loops
                    }

                    Fighter.Fight.EndSequence(SequenceTypeEnum.SEQUENCE_MOVE);
                }

                var i = 0;
                while (Fighter.CanCastSpell(cast.Spell, cast.TargetCell) == SpellCastResult.OK && i <= MaxCastLimit)
                {
                    if (!Fighter.CastSpell(cast.Spell, cast.TargetCell))
                        break;

                    i++;
                }
            }

            if (!Fighter.CanMove()) 
                return;


            foreach (var action in new MoveNearTo(Fighter, Environment.GetNearestEnnemy()).Execute(this))
            {

            }
        }

        public void Log(string log, params object[] args)
        {
            logger.Debug("Brain " + Fighter + " : " + log, args);

            if (DebugMode)
                Fighter.Say(string.Format(log, args));
        }
    }
}