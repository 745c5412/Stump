using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NLog;
using Stump.Core.Mathematics;
using Stump.Core.Pool;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Fights.Sequences;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Game.Fights.Triggers;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Game.Maps.Pathfinding;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Game.Spells.Casts;
using Stump.Server.WorldServer.Handlers.Actions;
using Stump.Server.WorldServer.Handlers.Basic;
using Stump.Server.WorldServer.Handlers.Context;

namespace Stump.Server.WorldServer.Game.Fights
{
    public abstract class FightBase : WorldObjectsContext
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        protected readonly ReversedUniqueIdProvider m_contextualIdProvider = new ReversedUniqueIdProvider(0);
        protected readonly UniqueIdProvider m_triggerIdProvider = new UniqueIdProvider();

        public FightBase(int id, Map fightMap, FightTeam defendersTeam, FightTeam challengersTeam)
        {
            Id = id;
            Map = fightMap;
            State = FightState.NotStarted;

            ChallengersTeam = challengersTeam;
            DefendersTeam = defendersTeam;
            
            ChallengersTeam.FighterAdded += OnFighterAdded;
            ChallengersTeam.FighterRemoved += OnFighterRemoved;

            DefendersTeam.FighterAdded += OnFighterAdded;
            DefendersTeam.FighterRemoved += OnFighterRemoved;

            Teams = new[] {ChallengersTeam, DefendersTeam};
            TimeLine = new TimeLine(this);
        }

        public int Id
        {
            get;
        }

        public Map Map
        {
            get;
        }

        public override Cell[] Cells => Map.Cells;

        public abstract FightTypeEnum FightType
        {
            get;
        }

        public FightState State
        {
            get;
            protected set;
        }

        public FightTeam ChallengersTeam
        {
            get;
            private set;
        }

        public FightTeam DefendersTeam
        {
            get;
            private set;
        }

        public FightTeam[] Teams
        {
            get;
        }

        public FightTeam Winners
        {
            get;
            protected set;
        }

        public FightTeam Losers
        {
            get;
            protected set;
        }

        public bool Draw
        {
            get;
            protected set;
        }

        public TimeLine TimeLine
        {
            get;
            private set;
        }

        public ReadOnlyCollection<FightActor> Fighters => TimeLine.Fighters.AsReadOnly();


        protected override IReadOnlyCollection<WorldObject> Objects => Fighters;

        public FightActor FighterPlaying => TimeLine.Current;

        #region Buffs

        private List<Buff> m_buffs = new List<Buff>();

        public ReadOnlyCollection<Buff> Buffs => m_buffs.AsReadOnly();

        protected virtual void OnBuffAdded(FightActor target, Buff buff)
        {
            m_buffs.Add(buff);
        }

        protected virtual void OnBuffRemoved(FightActor target, Buff buff)
        {
            m_buffs.Remove(buff);
        }

        #endregion

        #region Turn Management
                
        public virtual bool CheckFightEnd(bool endFight = true)
        {
            if (!ChallengersTeam.AreAllDead() && !DefendersTeam.AreAllDead())
                return false;

            if (endFight)
                EndFight();

            return true;
        }

        public abstract void EndFight();


        protected virtual void OnWinnersDetermined(FightTeam winners, FightTeam losers, bool draw)
        {
        }

        protected virtual void DeterminsWinners()
        {
            if (DefendersTeam.AreAllDead() && !ChallengersTeam.AreAllDead())
            {
                Winners = ChallengersTeam;
                Losers = DefendersTeam;
                Draw = false;
            }
            else if (!DefendersTeam.AreAllDead() && ChallengersTeam.AreAllDead())
            {
                Winners = DefendersTeam;
                Losers = ChallengersTeam;
                Draw = false;
            }
            else Draw = true;

            OnWinnersDetermined(Winners, Losers, Draw);
        }

        public virtual void StartTurn()
        {
            if (State != FightState.Fighting)
                return;

            if (!CheckFightEnd())
            {
                OnTurnStarted();
            }
        }


        protected virtual void OnNewRound()
        {

        }



        protected virtual bool OnTurnStarted()
        {
            ResetSequences();

            if (TimeLine.NewRound)
                OnNewRound();

            using (StartSequence(SequenceTypeEnum.SEQUENCE_TURN_START))
            {
                if (!FighterPlaying.IsSummoned())
                {
                    FighterPlaying.DecrementAllCastedBuffsDuration();
                    FighterPlaying.DecrementSummonsCastedBuffsDuration();
                }

                foreach (var passedFighter in TimeLine.PassedActors)
                {
                    passedFighter.DecrementAllCastedBuffsDuration();
                    passedFighter.DecrementSummonsCastedBuffsDuration();
                }

                DecrementGlyphDuration(FighterPlaying);
                TriggerMarks(FighterPlaying.Cell, FighterPlaying, TriggerType.OnTurnBegin);
                FighterPlaying.TriggerBuffs(FighterPlaying, BuffTriggerType.OnTurnBegin);
            }

            // can die with triggers
            if (CheckFightEnd())
                return false;

            if (FighterPlaying.MustSkipTurn())
            {
                PassTurn();
                return false;
            }

            FighterPlaying.TurnStartPosition = FighterPlaying.Position.Clone();
            return true;
        }

        public virtual bool StopTurn()
        {
            if (State != FightState.Fighting)
                return false;

            if (CheckFightEnd())
                return false;

            OnTurnStopped();
            return true;
        }

        protected virtual bool OnTurnStopped()
        {
            using (StartSequence(SequenceTypeEnum.SEQUENCE_TURN_END))
            {
                if (FighterPlaying.IsAlive())
                {
                    FighterPlaying.TriggerBuffs(FighterPlaying, BuffTriggerType.OnTurnEnd);
                    FighterPlaying.TriggerBuffsRemovedOnTurnEnd();
                    TriggerMarks(FighterPlaying.Cell, FighterPlaying, TriggerType.OnTurnEnd);
                }
            }

            // can die with triggers
            if (CheckFightEnd())
                return false;

            if (IsSequencing)
                EndAllSequences();

            return true;
        }

        protected virtual bool SelectNextFighter()
        {
            if (State != FightState.Fighting)
                return false;

            if (CheckFightEnd())
                return false;

            FighterPlaying?.ResetUsedPoints();

            if (!TimeLine.SelectNextFighter())
            {
                if (!CheckFightEnd())
                {
                    logger.Error("Something goes wrong : no more actors are available to play but the fight is not ended");
                }

                return false;
            }

            return true;
        }

        protected virtual bool PassTurn()
        {
            if (!SelectNextFighter())
                return false;
            
          
            if (OnTurnPassed())
                StartTurn();

            return true;
        }

        protected virtual bool OnTurnPassed()
        {
            if (IsSequencing)
                EndAllSequences();

            return true;
        }

        #endregion Turn Management

        #region Movement

        protected virtual void OnStartMoving(ContextActor actor, Path path)
        {
            var fighter = actor as FightActor;
            var character = actor is CharacterFighter ? (actor as CharacterFighter).Character : null;

            if (fighter != null && !fighter.IsFighterTurn())
                return;

            if (!(path is FightPath))
                return;

            var fightPath = (FightPath)path;
            using (StartMoveSequence(fightPath))
            {
                foreach (var tackle in fightPath.Tackles)
                {
                    OnTackled(fighter, tackle);
                }

                actor.StopMove();
            }
        }

        public event Action<FightActor, int, int> Tackled;

        protected virtual void OnTackled(FightActor actor, FightTackle tackle)
        {
            if (actor.MP - tackle.TackledMP < 0)
            {
                logger.Error("Cannot apply tackle : mp tackled ({0}) > available mp ({1})", tackle.TackledMP, actor.MP);
                return;
            }
            
            actor.LostAP((short)tackle.TackledAP, actor);
            actor.LostMP((short)tackle.TackledMP, actor);

            actor.TriggerBuffs(actor, BuffTriggerType.OnTackled);

            foreach (var tackler in tackle.Tacklers)
                tackler.TriggerBuffs(tackler, BuffTriggerType.OnTackle);

            Tackled?.Invoke(actor, tackle.TackledAP, tackle.TackledMP);
        }

        protected virtual void OnStopMoving(ContextActor actor, Path path, bool canceled)
        {
            var fighter = actor as FightActor;

            if (fighter != null && !fighter.IsFighterTurn())
                return;

            if (canceled)
                return; // error, mouvement shouldn't be canceled in a fight.

            if (fighter == null)
                return;

            fighter.UseMP((short)path.MPCost);
        }

        protected virtual void OnPositionChanged(ContextActor actor, ObjectPosition objectPosition)
        {
            var fighter = actor as FightActor;

            if (fighter == null)
                return;

            TriggerMarks(fighter.Cell, fighter, TriggerType.MOVE);
        }

        #endregion Movement

        #region Spells
        
        protected virtual void OnSpellCasted(FightActor caster, SpellCastHandler castHandler)
        {
            CheckFightEnd();
        }

        protected virtual void OnSpellCastFailed(FightActor caster, SpellCastInformations cast)
        {
        }

        #endregion Spells

        #region Triggers

        private readonly List<MarkTrigger> m_triggers = new List<MarkTrigger>();

        public ReadOnlyCollection<MarkTrigger> Triggers => m_triggers.AsReadOnly();

        public bool ShouldTriggerOnMove(Cell cell, FightActor actor)
            => m_triggers.Any(entry => entry.TriggerType.HasFlag(TriggerType.MOVE) && entry.StopMovement && entry.ContainsCell(cell) && entry.CanTrigger(actor));

        public MarkTrigger[] GetTriggersByCell(Cell cell) => m_triggers.Where(entry => entry.ContainsCell(cell)).ToArray();

        public MarkTrigger[] GetTriggers(Cell cell) => m_triggers.Where(entry => entry.CenterCell.Id == cell.Id).ToArray();

        public void AddTriger(MarkTrigger trigger)
        {
            trigger.Triggered += OnMarkTriggered;
            m_triggers.Add(trigger);

            OnTriggerAdded(trigger);

            if (!trigger.TriggerType.HasFlag(TriggerType.CREATION))
                return;

            var fighters = GetAllFighters(trigger.GetCells());
            foreach (var fighter in fighters)
                trigger.Trigger(fighter);
        }

        protected virtual void OnTriggerAdded(MarkTrigger trigger)
        {
        }
        public void RemoveTrigger(MarkTrigger trigger)
        {
            trigger.Triggered -= OnMarkTriggered;
            m_triggers.Remove(trigger);
            trigger.NotifyRemoved();

            OnTriggerRemoved(trigger);
        }

        protected virtual void OnTriggerRemoved(MarkTrigger trigger)
        {
        }

        public void TriggerMarks(Cell cell, FightActor trigger, TriggerType triggerType)
        {
            var triggers =
                m_triggers.Where(markTrigger => markTrigger.TriggerType.HasFlag(triggerType) && markTrigger.ContainsCell(cell) && markTrigger.CanTrigger(trigger))
                    .OrderByDescending(x => x.Priority)
                    .ToArray();

            foreach (var markTrigger in triggers.OfType<Trap>())
                markTrigger.WillBeTriggered = true;

            // we use a copy 'cause a trigger can be deleted when a fighter die with it
            foreach (var markTrigger in triggers)
            {
                if (!trigger.CanPlay() && (triggerType == TriggerType.OnTurnBegin || triggerType == TriggerType.OnTurnEnd)
                    && (markTrigger is Wall || markTrigger is Glyph))
                    continue;

                using (StartSequence(SequenceTypeEnum.SEQUENCE_GLYPH_TRAP))
                    markTrigger.Trigger(trigger, cell);
            }
        }

        public void DecrementGlyphDuration(FightActor caster)
        {
            var triggersToRemove = m_triggers.Where(trigger => trigger.Caster == caster).
                Where(trigger => trigger.DecrementDuration()).ToList();

            if (triggersToRemove.Count == 0)
                return;

            using (StartSequence(SequenceTypeEnum.SEQUENCE_GLYPH_TRAP))
            {
                foreach (var trigger in triggersToRemove)
                {
                    RemoveTrigger(trigger);
                }
            }
        }

        public int PopNextTriggerId() => m_triggerIdProvider.Pop();

        public void FreeTriggerId(int id)
        {
            m_triggerIdProvider.Push(id);
        }

        protected virtual void OnMarkTriggered(MarkTrigger markTrigger, FightActor trigger, Spell triggerSpell)
        {
        }

        #endregion Triggers

        #region Sequences

        private int m_nextSequenceId = 1;
        private readonly List<FightSequence> m_sequencesRoot = new List<FightSequence>();

        public FightSequence CurrentSequence
        {
            get;
            private set;
        }

        public FightSequence CurrentRootSequence
        {
            get;
            private set;
        }

        public bool IsSequencing => CurrentRootSequence != null && !CurrentRootSequence.Ended;

        public DateTime LastSequenceEndTime => m_sequencesRoot.Count > 0 ? m_sequencesRoot.Max(x => x.EndTime) : DateTime.Now;

        public FightSequence StartMoveSequence(FightPath path)
        {
            var id = m_nextSequenceId++;
            var sequence = new FightMoveSequence(id, TimeLine.Current, path);
            StartSequence(sequence);
            return sequence;
        }

        public FightSequence StartSequence(SequenceTypeEnum type)
        {
            var id = m_nextSequenceId++;
            var sequence = new FightSequence(id, type, TimeLine.Current);
            StartSequence(sequence);
            return sequence;
        }

        private void StartSequence(FightSequence sequence)
        {
            if (!IsSequencing)
            {
                if (sequence.Parent != null)
                {
                    logger.Error($"Sequence {sequence.Type} is a root and cannot have a parent");
                }

                m_sequencesRoot.Add(sequence);
                CurrentRootSequence = sequence;
            }
            else
                CurrentSequence.AddChildren(sequence);


            CurrentSequence = sequence;
            OnSequenceStarted(sequence);
        }

        protected virtual void OnSequenceStarted(FightSequence sequence)
        {
        }

        public virtual void OnSequenceEnded(FightSequence sequence)
        {
            if (CurrentSequence != sequence)
            {
                logger.Error($"Sequence incoherence {sequence} instead of {CurrentSequence}");
            }

            CurrentSequence = sequence.Parent;
        }


        public void EndAllSequences()
        {
            foreach (var sequence in m_sequencesRoot)
                sequence.EndSequence();
        }

        public void ResetSequences()
        {
            m_sequencesRoot.Clear();
            CurrentSequence = null;
            m_nextSequenceId = 1;
        }

        public virtual bool AcknowledgeAction(CharacterFighter fighter, int sequenceId)
        {
            return m_sequencesRoot.Any(x => x.Acknowledge(sequenceId, fighter));
        }

        #endregion Sequences

        #region Add/Remove Fighters


        protected void UnBindFightersEvents()
        {
            foreach (var fighter in Fighters)
            {
                UnBindFighterEvents(fighter);
            }
        }

        protected virtual void UnBindFighterEvents(FightActor actor)
        {
            actor.StartMoving -= OnStartMoving;
            actor.StopMoving -= OnStopMoving;
            actor.PositionChanged -= OnPositionChanged;
            actor.SpellCasted -= OnSpellCasted;
            actor.BuffAdded -= OnBuffAdded;
            actor.BuffRemoved -= OnBuffRemoved;
        }

        protected void BindFightersEvents()
        {
            foreach (var fighter in Fighters)
            {
                BindFighterEvents(fighter);
            }
        }

        protected virtual void BindFighterEvents(FightActor actor)
        {
            if (State == FightState.Fighting)
            {
                actor.StartMoving += OnStartMoving;
                actor.StopMoving += OnStopMoving;
                actor.PositionChanged += OnPositionChanged;
                actor.SpellCasted += OnSpellCasted;
                actor.SpellCastFailed += OnSpellCastFailed;
                actor.BuffAdded += OnBuffAdded;
                actor.BuffRemoved += OnBuffRemoved;
            }
        }

        protected virtual void OnFighterAdded(FightTeam team, FightActor actor)
        {
            if (State == FightState.Ended)
                return;

            BindFighterEvents(actor);

            if (actor.IsSummoned())
            {
                TimeLine.InsertFighter(actor, TimeLine.Fighters.IndexOf(actor.Summoner) + 1);
                actor.OnGetAlive();
            }
            else
            {
                TimeLine.Fighters.Add(actor);
            }

            if (actor is CharacterFighter)
                OnCharacterAdded(actor as CharacterFighter);
        }

        protected virtual void OnCharacterAdded(CharacterFighter fighter)
        {
        }

        protected virtual void OnFighterRemoved(FightTeam team, FightActor actor)
        {
            TimeLine.RemoveFighter(actor);
            UnBindFighterEvents(actor);

            if (actor is CharacterFighter)
                OnCharacterRemoved(actor as CharacterFighter);
        }

        protected virtual void OnCharacterRemoved(CharacterFighter fighter)
        {
        }

        #endregion

        #region Get Methods

        public bool IsCellFree(Cell cell)
        {
            return cell.Walkable && !cell.NonWalkableDuringFight && GetOneFighter(cell) == null;
        }


        public sbyte GetNextContextualId()
        {
            return (sbyte) m_contextualIdProvider.Pop();
        }

        public void FreeContextualId(sbyte id)
        {
            m_contextualIdProvider.Push(id);
        }

        public T GetRandomFighter<T>() where T : FightActor
        {
            var fighters = Fighters.Where(x => x is T && x.IsAlive()).ToArray();

            if (!fighters.Any())
                return null;

            var random = new CryptoRandom().Next(0, fighters.Count());

            return fighters[random] as T;
        }

        public FightActor GetOneFighter(int id)
        {
            return Fighters.FirstOrDefault(entry => entry.Id == id);
        }

        public FightActor GetOneFighter(Cell cell)
        {
            return Fighters.FirstOrDefault(entry => entry.IsAlive() && entry.Cell.Id == cell.Id);
        }

        public FightActor GetOneFighter(Predicate<FightActor> predicate)
        {
            var entries = Fighters.Where(entry => predicate(entry));

            var fightActors = entries as FightActor[] ?? entries.ToArray();
            return fightActors.Count() != 0 ? null : fightActors.FirstOrDefault();
        }

        public T GetOneFighter<T>(int id) where T : FightActor
        {
            return Fighters.OfType<T>().FirstOrDefault(entry => entry.Id == id);
        }

        public T GetOneFighter<T>(Cell cell) where T : FightActor
        {
            return Fighters.OfType<T>().FirstOrDefault(entry => entry.IsAlive() && Equals(entry.Position.Cell, cell));
        }

        public T GetOneFighter<T>(Predicate<T> predicate) where T : FightActor
        {
            return Fighters.OfType<T>().FirstOrDefault(entry => predicate(entry));
        }

        public T GetFirstFighter<T>(int id) where T : FightActor
        {
            return Fighters.OfType<T>().FirstOrDefault(entry => entry.Id == id);
        }

        public T GetFirstFighter<T>(Cell cell) where T : FightActor
        {
            return Fighters.OfType<T>().FirstOrDefault(entry => entry.IsAlive() && Equals(entry.Position.Cell, cell));
        }

        public T GetFirstFighter<T>(Predicate<T> predicate) where T : FightActor
        {
            return Fighters.OfType<T>().FirstOrDefault(entry => predicate(entry));
        }

        public List<FightActor> GetAllFightersInLine(MapPoint startCell, int range, DirectionsEnum direction)
        {
            var fighters = new List<FightActor>();
            var nextCell = startCell.GetNearestCellInDirection(direction);
            var i = 0;

            while (nextCell != null && Map.Cells[nextCell.CellId].Walkable && !Map.Cells[nextCell.CellId].NonWalkableDuringFight && i < range)
            {
                var fighter = GetOneFighter(Map.Cells[nextCell.CellId]);

                if (fighter == null && fighters.Any())
                    break;

                if (fighter != null)
                    fighters.Add(fighter);

                nextCell = nextCell.GetNearestCellInDirection(direction);
                i++;
            }

            return fighters;
        }

        public ReadOnlyCollection<FightActor> GetAllFighters()
        {
            return Fighters;
        }

        public IEnumerable<FightActor> GetAllFighters(Cell[] cells)
        {
            return GetAllFighters<FightActor>(entry => entry.IsAlive() && cells.Contains(entry.Position.Cell));
        }

        public IEnumerable<FightActor> GetAllFighters(IEnumerable<Cell> cells)
        {
            return GetAllFighters(cells.ToArray());
        }

        public IEnumerable<FightActor> GetAllFighters(Predicate<FightActor> predicate)
        {
            return Fighters.Where(entry => predicate(entry));
        }

        public IEnumerable<T> GetAllFighters<T>() where T : FightActor
        {
            return Fighters.OfType<T>();
        }

        public IEnumerable<T> GetAllFighters<T>(Predicate<T> predicate) where T : FightActor
        {
            return Fighters.OfType<T>().Where(entry => predicate(entry));
        }


        public FightCommonInformations GetFightCommonInformations()
        {
            return new FightCommonInformations(Id,
                (sbyte) FightType,
                Teams.Select(entry => entry.GetFightTeamInformations()),
                Teams.Select(entry => entry.BladePosition.Cell.Id),
                Teams.Select(entry => entry.GetFightOptionsInformations()));
        }

        public IEnumerable<NamedPartyTeam> GetPartiesName()
        {
            var redParty = ChallengersTeam.GetTeamParty();
            var blueParty = DefendersTeam.GetTeamParty();

            var parties = new[] {redParty, blueParty};
            return parties.Select((x, i) => Tuple.Create(i, x?.Name)).Where(x => x.Item2 != null).Select(x => new NamedPartyTeam((sbyte) x.Item1, x.Item2));
        }

        public IEnumerable<NamedPartyTeamWithOutcome> GetPartiesNameWithOutcome()
        {
            var redParty = ChallengersTeam.GetTeamParty();
            var blueParty = ChallengersTeam.GetTeamParty();

            var parties = new[] {redParty, blueParty};
            return parties.Select((x, i) => Tuple.Create(i, x?.Name)).Where(x => x.Item2 != null).
                Select(x => new NamedPartyTeamWithOutcome(new NamedPartyTeam((sbyte) x.Item1, x.Item2), (short) Teams[x.Item1].GetOutcome()));
        }

        #endregion Get Methods
    }
}