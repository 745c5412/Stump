﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NLog;
using Stump.Core.Attributes;
using Stump.Core.Extensions;
using Stump.Core.Mathematics;
using Stump.Core.Pool;
using Stump.Core.Timers;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Enums.Custom;
using Stump.DofusProtocol.Types;
using Stump.ORM.SubSonic.Extensions;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Fights.Challenges;
using Stump.Server.WorldServer.Game.Fights.Results;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Game.Fights.Triggers;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Game.Maps.Pathfinding;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Handlers.Actions;
using Stump.Server.WorldServer.Handlers.Basic;
using Stump.Server.WorldServer.Handlers.Characters;
using Stump.Server.WorldServer.Handlers.Context;
using FightLoot = Stump.Server.WorldServer.Game.Fights.Results.FightLoot;
using Stump.Core.Collections;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Handlers.Context.RolePlay;

namespace Stump.Server.WorldServer.Game.Fights
{
    public delegate void FightWinnersDelegate(IFight fight, FightTeam winners, FightTeam losers, bool draw);
    public delegate void ResultsGeneratedDelegate(IFight fight, List<IFightResult> results);

    public interface IFight : ICharacterContainer
    {
        int Id
        {
            get;
        }

        Map Map
        {
            get;
        }

        Cell[] Cells
        {
            get;
        }

        FightTypeEnum FightType
        {
            get;
        }

        bool IsPvP
        {
            get;
        }

        bool IsMultiAccountRestricted
        {
            get;
        }

        FightState State
        {
            get;
        }

        bool IsStarted
        {
            get;
        }

        DateTime CreationTime
        {
            get;
        }

        DateTime StartTime
        {
            get;
        }

        short AgeBonus
        {
            get;
        }

        FightTeam ChallengersTeam
        {
            get;
        }

        FightTeam DefendersTeam
        {
            get;
        }

        FightTeam Winners
        {
            get;
        }

        FightTeam Losers
        {
            get;
        }

        bool Draw
        {
            get;
        }

        TimeLine TimeLine
        {
            get;
        }
        ReadOnlyCollection<FightActor> Fighters
        {
            get;
        }

        ReadOnlyCollection<FightActor> Leavers
        {
            get;
        }

        ReadOnlyCollection<FightSpectator> Spectators
        {
            get;
        }

        FightActor FighterPlaying
        {
            get;
        }

        DefaultChallenge Challenge
        {
            get;
        }

        DateTime TurnStartTime
        {
            get;
        }

        ReadyChecker ReadyChecker
        {
            get;
        }

        bool SpectatorClosed
        {
            get;
        }

        bool BladesVisible
        {
            get;
        }

        FightLoot TaxCollectorLoot
        {
            get;
        }

        SequenceTypeEnum Sequence
        {
            get;
        }

        bool IsSequencing
        {
            get;
        }

        bool WaitAcknowledgment
        {
            get;
        }

        bool IsDeathTemporarily
        {
            get;
        }

        bool CanKickPlayer
        {
            get;
        }

        /// <summary>
        /// Do not modify, just read
        /// </summary>
        WorldClientCollection Clients
        {
            get;
        }

        /// <summary>
        /// Do not modify, just read
        /// </summary>
        WorldClientCollection SpectatorClients
        {
            get;
        }

        bool AIDebugMode
        {
            get;
            set;
        }

        bool Freezed
        {
            get;
            set;
        }

        event Action<IFight> FightStarted;
        event Action<IFight> FightEnded;
        
        void Initialize();
        void StartFighting();
        bool CheckFightEnd();
        void CancelFight();
        void EndFight();
        event FightWinnersDelegate WinnersDetermined;
        event ResultsGeneratedDelegate ResultsGenerated;
        event Action<IFight> GeneratingResults;
        void StartPlacement();
        void ShowBlades();
        void HideBlades();
        void UpdateBlades(FightTeam team);
        bool FindRandomFreeCell(FightActor fighter, out Cell cell, bool placement = true);
        bool RandomnizePosition(FightActor fighter);
        void RandomnizePositions(FightTeam team);
        DirectionsEnum FindPlacementDirection(FightActor fighter);
        bool KickFighter(FightActor kicker, FightActor fighter);

        /// <summary>
        ///   Check if a character can change position (before the fight is started).
        /// </summary>
        /// <param name = "fighter"></param>
        /// <param name="cell"></param>
        /// <returns>If change is possible</returns>
        bool CanChangePosition(FightActor fighter, Cell cell);

        void ToggleSpectatorClosed(Character character, bool state);
        bool CanSpectatorJoin(Character spectator);
        bool AddSpectator(FightSpectator spectator);
        void RemoveSpectator(FightSpectator spectator);
        void RemoveAllSpectators();
        void StartTurn();
        event Action<IFight, FightActor> TurnStarted;
        void StopTurn();
        event Action<IFight, FightActor> BeforeTurnStopped;
        event Action<IFight, FightActor> TurnStopped;
        event Action<FightActor, int, int> Tackled;
        IEnumerable<Buff> GetBuffs();
        void UpdateBuff(Buff buff, bool updateAction = true);
        bool StartSequence(SequenceTypeEnum sequenceType);
        bool EndSequence(SequenceTypeEnum sequenceType, bool force = false);
        void EndAllSequences();
        void AcknowledgeAction();
        IEnumerable<MarkTrigger> GetTriggers();
        bool ShouldTriggerOnMove(Cell cell, FightActor actor);
        MarkTrigger[] GetTriggersByCell(Cell cell);
        MarkTrigger[] GetTriggers(Cell cell);
        void AddTriger(MarkTrigger trigger);
        void RemoveTrigger(MarkTrigger trigger);
        void TriggerMarks(Cell cell, FightActor trigger, TriggerType triggerType);
        void DecrementGlyphDuration(FightActor caster);
        int PopNextTriggerId();
        void FreeTriggerId(int id);
        void SetChallenge(DefaultChallenge challenge);
        int GetChallengeBonus();
        IEnumerable<Character> GetAllCharacters();
        IEnumerable<Character> GetAllCharacters(bool withSpectators = false);
        void ForEach(Action<Character> action);
        void ForEach(Action<Character> action, bool withSpectators = false);
        void ForEach(Action<Character> action, Character except, bool withSpectators = false);
        bool IsCellFree(Cell cell);
        TimeSpan GetFightDuration();
        TimeSpan GetTurnTimeLeft();
        TimeSpan GetPlacementTimeLeft();
        sbyte GetNextContextualId();
        void FreeContextualId(sbyte id);
        FightActor GetOneFighter(int id);
        FightActor GetOneFighter(Cell cell);
        FightActor GetOneFighter(Predicate<FightActor> predicate);
        T GetRandomFighter<T>() where T : FightActor;
        T GetOneFighter<T>(int id) where T : FightActor;
        T GetOneFighter<T>(Cell cell) where T : FightActor;
        T GetOneFighter<T>(Predicate<T> predicate) where T : FightActor;
        T GetFirstFighter<T>(int id) where T : FightActor;
        T GetFirstFighter<T>(Cell cell) where T : FightActor;
        T GetFirstFighter<T>(Predicate<T> predicate) where T : FightActor;
        List<FightActor> GetAllFightersInLine(MapPoint startCell, int range, DirectionsEnum direction);
        ReadOnlyCollection<FightActor> GetAllFighters();
        ReadOnlyCollection<FightActor> GetLeavers();
        CharacterFighter GetLeaver(int characterId);
        ReadOnlyCollection<FightSpectator> GetSpectators();
        IEnumerable<Character> GetCharactersAndSpectators();
        IEnumerable<FightActor> GetFightersAndLeavers();
        IEnumerable<FightActor> GetAllFighters(Cell[] cells);
        IEnumerable<FightActor> GetAllFighters(IEnumerable<Cell> cells);
        IEnumerable<FightActor> GetAllFighters(Predicate<FightActor> predicate);
        IEnumerable<T> GetAllFighters<T>() where T : FightActor;
        IEnumerable<T> GetAllFighters<T>(Predicate<T> predicate) where T : FightActor;
        IEnumerable<int> GetDeadFightersIds();
        IEnumerable<int> GetAliveFightersIds();
        FightCommonInformations GetFightCommonInformations();
        FightExternalInformations GetFightExternalInformations();
        bool CanBeSeen(MapPoint from, MapPoint to, bool throughEntities = false, WorldObject except = null);

        void RejoinFightFromDisconnection(CharacterFighter character);
        void RefreshActor(FightActor actor);
    }

    // this is necessary since we can't read static field dynamically in a generic class
    public static class FightConfiguration
    {
        
        [Variable]
        public static int PlacementPhaseTime = 30000;

        /// <summary>
        ///   Delay for player's turn
        /// </summary>
        [Variable]
        public static int TurnTime = 15000;

        /// <summary>
        ///   Max Delay for player's turn
        /// </summary>
        [Variable]
        public static int MaxTurnTime = 60000;

        /// <summary>
        ///   Delay before force turn to end
        /// </summary>
        [Variable]
        public static int TurnEndTimeOut = 5000;

        /// <summary>
        ///   Delay before force turn to end
        /// </summary>
        [Variable]
        public static int EndFightTimeOut = 10000;

        [Variable]
        public static int TurnsBeforeDisconnection = 20;
    }

    public abstract class Fight<TBlueTeam,TRedTeam> : WorldObjectsContext, IFight
        where TRedTeam : FightTeam
        where TBlueTeam : FightTeam
    {
        protected readonly Logger logger = LogManager.GetCurrentClassLogger();

        #region Config


        #endregion

        #region Events

        #endregion

        #region Constructor

        protected Fight(int id, Map fightMap, TBlueTeam defendersTeam, TRedTeam challengersTeam)
        {
            Id = id;
            Map = fightMap;
            DefendersTeam = defendersTeam;
            DefendersTeam.Fight = this;
            ChallengersTeam = challengersTeam;
            ChallengersTeam.Fight = this;
            m_teams = new[] {(FightTeam)ChallengersTeam, DefendersTeam};

            TimeLine = new TimeLine(this);
            m_leavers = new List<FightActor>();
            m_spectators = new List<FightSpectator>();

            DefendersTeam.FighterAdded += OnFighterAdded;
            DefendersTeam.FighterRemoved += OnFighterRemoved;
            ChallengersTeam.FighterAdded += OnFighterAdded;
            ChallengersTeam.FighterRemoved += OnFighterRemoved;

            CreationTime = DateTime.Now;
            TaxCollectorLoot = new FightLoot();
        }

        #endregion

        #region Properties

        protected readonly ReversedUniqueIdProvider m_contextualIdProvider = new ReversedUniqueIdProvider(0);
        protected readonly UniqueIdProvider m_triggerIdProvider = new UniqueIdProvider();

        protected readonly List<Buff> m_buffs = new List<Buff>();

        protected TimedTimerEntry m_placementTimer;
        protected TimedTimerEntry m_turnTimer;

        private bool m_isInitialized;
        private bool m_disposed;

        protected FightTeam[] m_teams;

        public int Id
        {
            get;
            private set;
        }

        public Map Map
        {
            get;
            private set;
        }

        public override Cell[] Cells
        {
            get
            {
                return Map.Cells;
            }
        }

        protected override IReadOnlyCollection<WorldObject> Objects
        {
            get
            {
                return Fighters;
            }
        }

        public abstract FightTypeEnum FightType
        {
            get;
        }

        public abstract bool IsPvP
        {
            get;
        }

        public virtual bool IsMultiAccountRestricted
        {
            get { return false; }
        }

        public FightState State
        {
            get;
            private set;
        }

        public bool IsStarted
        {
            get;
            private set;
        }

        public DateTime CreationTime
        {
            get;
            private set;
        }

        public DateTime StartTime
        {
            get;
            private set;
        }

        public short AgeBonus
        {
            get;
            protected set;
        }

        FightTeam IFight.ChallengersTeam
        {
            get { return ChallengersTeam; }
        }
        FightTeam IFight.DefendersTeam
        {
            get { return DefendersTeam; }
        }

        public TRedTeam ChallengersTeam
        {
            get;
            private set;
        }

        public TBlueTeam DefendersTeam
        {
            get;
            private set;
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

        public FightActor FighterPlaying => TimeLine.Current;

        public DefaultChallenge Challenge
        {
            get;
            private set;
        }

        public DateTime TurnStartTime
        {
            get;
            protected set;
        }

        public ReadyChecker ReadyChecker
        {
            get;
            protected set;
        }

        public ReadOnlyCollection<FightActor> Fighters => TimeLine.Fighters.AsReadOnly();

        public ReadOnlyCollection<FightActor> Leavers
        {
            get { return m_leavers.AsReadOnly(); }
        }

        public ReadOnlyCollection<FightSpectator> Spectators
        {
            get { return m_spectators.AsReadOnly(); }
        }

        public bool SpectatorClosed
        {
            get;
            private set;
        }

        public bool BladesVisible
        {
            get;
            private set;
        }

        public FightLoot TaxCollectorLoot
        {
            get;
            private set;
        }

        public virtual bool IsDeathTemporarily
        {
            get { return false; }
        }

        public virtual bool CanKickPlayer
        {
            get { return true; }
        }

        public bool AIDebugMode
        {
            get;
            set;
        }


        #endregion

        #region Phases

        protected void SetFightState(FightState state)
        {
            UnBindFightersEvents();
            State = state;
            BindFightersEvents();

            OnStateChanged();
        }

        protected virtual void OnStateChanged()
        {
            if (State != FightState.Placement && BladesVisible)
                HideBlades();
        }

        public void Initialize()
        {
            if (m_isInitialized)
                return;

            ProcessInitialization();

            m_isInitialized = true;
        }

        protected virtual void ProcessInitialization()
        {
        }

        public virtual void StartFighting()
        {
            if (State != FightState.Placement &&
                State != FightState.NotStarted) // we can imagine a fight without placement phase
                return;

            SetFightState(FightState.Fighting);
            StartTime = DateTime.Now;
            IsStarted = true;

            HideBlades();

            TimeLine.OrderLine();

            ContextHandler.SendGameEntitiesDispositionMessage(Clients, GetAllFighters());
            ContextHandler.SendGameFightStartMessage(Clients);
            ContextHandler.SendGameFightTurnListMessage(Clients, this);
            ForEach(entry => ContextHandler.SendGameFightSynchronizeMessage(entry.Client, this), true);
            OnFightStarted();

            StartTurn();
        }
        

        #region EndFight

        public bool CheckFightEnd()
        {
            if (!ChallengersTeam.AreAllDead() && !DefendersTeam.AreAllDead())
                return false;

            EndFight();
            return true;
        }

        public void CancelFight()
        {
            if (!CanCancelFight())
                return;

            if (State != FightState.Placement)
            {
                EndFight();
                return;
            }

            SetFightState(FightState.Ended);

            ContextHandler.SendGameFightEndMessage(Clients, this);

            foreach (var character in GetCharactersAndSpectators())
            {
                character.RejoinMap();
            }

            Dispose();
        }

        public void EndFight()
        {
            if (State == FightState.Placement)
                CancelFight();

            if (State == FightState.Ended)
                return;

            SetFightState(FightState.Ended);

            if (m_turnTimer != null)
                m_turnTimer.Dispose();

            EndAllSequences();

            if (ReadyChecker != null)
            {
                ReadyChecker.Cancel();
            }

            ReadyChecker = ReadyChecker.RequestCheck(this, OnFightEnded, actors => OnFightEnded());
        }

        public event Action<IFight> FightStarted;

        protected virtual void OnFightStarted()
        {
            foreach (var fighter in Fighters)
            {
                fighter.FightStartPosition = fighter.Position.Clone();
                fighter.MovementHistory.RegisterEntry(fighter.FightStartPosition.Cell);
            }

            var handler = FightStarted;
            if (handler != null)
                handler(this);
        }

        public event Action<IFight> FightEnded;
        protected virtual void OnFightEnded()
        {
            ReadyChecker = null;
            DeterminsWinners();
            GenerateResults();

            ApplyResults();

            ContextHandler.SendGameFightEndMessage(Clients, this, Results.Select(entry => entry.GetFightResultListEntry()));

            ResetFightersProperties();
            foreach (var character in GetCharactersAndSpectators())
            {
                character.RejoinMap();
            }

            Dispose();

            var handler = FightEnded;
            if (handler != null)
                handler(this);
        }

        public event FightWinnersDelegate WinnersDetermined;

        protected virtual void OnWinnersDetermined(FightTeam winners, FightTeam losers, bool draw)
        {
            var handler = WinnersDetermined;
            if (handler != null) handler(this, winners, losers, draw);
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

        protected void ResetFightersProperties()
        {
            foreach (var fighter in Fighters)
            {
                fighter.ResetFightProperties();
            }
        }


        public event Action<IFight> GeneratingResults;
        public event ResultsGeneratedDelegate ResultsGenerated;

        protected List<IFightResult> Results
        {
            get;
            set;
        } 

        protected void GenerateResults()
        {
            var handler = GeneratingResults;
            if (handler != null)
                handler(this);

            var results = GetResults();

            var handler2 = ResultsGenerated;
            if (handler2 != null)
                handler2(this, results);

            Results = results;
        }
        

        protected virtual List<IFightResult> GetResults()
        {
            return new List<IFightResult>();
        }

        protected virtual IEnumerable<IFightResult> GenerateLeaverResults(CharacterFighter leaver,
            out IFightResult leaverResult)
        {
            leaverResult = null;
            var list = new List<IFightResult>();
            foreach (var fighter in GetFightersAndLeavers().Where(entry => entry.HasResult))
            {
                var result =
                    fighter.GetFightResult(fighter.Team == leaver.Team
                        ? FightOutcomeEnum.RESULT_LOST
                        : FightOutcomeEnum.RESULT_VICTORY);

                if (fighter == leaver)
                    leaverResult = result;

                list.Add(result);
            }

            return list;
        }

        protected virtual void ApplyResults()
        {
            foreach (var fightResult in Results.Where(fightResult => !fightResult.HasLeft ||
                !(fightResult is FightPlayerResult) ||
                ((FightPlayerResult) fightResult).Fighter.IsDisconnected))
            {
                fightResult.Apply();
            }
        }

        protected void Dispose()
        {
            if (m_disposed)
                return;

            m_disposed = true;

            foreach (var fighter in Fighters)
            {
                fighter.Delete();
            }

            OnDisposed();

            UnBindFightersEvents();

            Map.RemoveFight(this);
            FightManager.Instance.Remove(this);
            GC.SuppressFinalize(this);
        }

        protected virtual void OnDisposed()
        {
            Clients.Dispose();

            if (ReadyChecker != null)
                ReadyChecker.Cancel();

            if (m_placementTimer != null)
                m_placementTimer.Dispose();

            if (m_turnTimer != null)
                m_turnTimer.Dispose();
        }

        #endregion

        #region Placement

        public virtual void StartPlacement()
        {
            if (State != FightState.NotStarted)
                return;

            SetFightState(FightState.Placement);

            RandomnizePositions(ChallengersTeam);
            RandomnizePositions(DefendersTeam);

            ShowBlades();
            Map.AddFight(this);
        }
        public void RefreshActor(FightActor actor)
        {
            if (actor.HasLeft())
                return;

            ForEach(entry => ContextHandler.SendGameFightShowFighterMessage(entry.Client, actor), true);
        }
        
        private void OnStatsRefreshed(Character character)
        {
            ForEach(entry => ContextHandler.SendGameFightShowFighterMessage(entry.Client, character.Fighter), true);
        }


        #region Blades

        private void FindBladesPlacement()
        {
            if (ChallengersTeam.Leader.MapPosition.Cell.Id != DefendersTeam.Leader.MapPosition.Cell.Id)
            {
                ChallengersTeam.BladePosition = ChallengersTeam.Leader.MapPosition.Clone();
                DefendersTeam.BladePosition = DefendersTeam.Leader.MapPosition.Clone();
            }
            else
            {
                var cell = Map.GetRandomAdjacentFreeCell(ChallengersTeam.Leader.MapPosition.Point);

                // if cell not found we superpose both blades
                if (cell == null)
                {
                    ChallengersTeam.BladePosition = ChallengersTeam.Leader.MapPosition.Clone();
                }
                else // else we take an adjacent cell
                {
                    var pos = ChallengersTeam.Leader.MapPosition.Clone();
                    pos.Cell = cell;
                    ChallengersTeam.BladePosition = pos;
                }

                DefendersTeam.BladePosition = DefendersTeam.Leader.MapPosition.Clone();
            }
        }

        public void ShowBlades()
        {
            if (BladesVisible || State != FightState.Placement)
                return;

            if (ChallengersTeam.BladePosition == null ||
                DefendersTeam.BladePosition == null)
                FindBladesPlacement();

            ContextHandler.SendGameRolePlayShowChallengeMessage(Map.Clients, this);

            ChallengersTeam.TeamOptionsChanged += OnTeamOptionsChanged;
            DefendersTeam.TeamOptionsChanged += OnTeamOptionsChanged;

            BladesVisible = true;
        }

        public void HideBlades()
        {
            if (!BladesVisible)
                return;

            ContextHandler.SendGameRolePlayRemoveChallengeMessage(Map.Clients, this);

            ChallengersTeam.TeamOptionsChanged -= OnTeamOptionsChanged;
            DefendersTeam.TeamOptionsChanged -= OnTeamOptionsChanged;

            BladesVisible = false;
        }

        public void UpdateBlades(FightTeam team)
        {
            if (!BladesVisible)
                return;

            ContextHandler.SendGameFightUpdateTeamMessage(Map.Clients, this, team);
        }

        void OnTeamOptionsChanged(FightTeam team, FightOptionsEnum option)
        {
            ContextHandler.SendGameFightOptionStateUpdateMessage(Clients, team, option, team.GetOptionState(option));
            ContextHandler.SendGameFightOptionStateUpdateMessage(Map.Clients, team, option, team.GetOptionState(option));
        }

        #endregion

        public virtual TimeSpan GetPlacementTimeLeft() => TimeSpan.Zero;

        #region Placement methods

        public bool FindRandomFreeCell(FightActor fighter, out Cell cell, bool placement = true)
        {
            var availableCells = fighter.Team.PlacementCells.Where(entry => GetOneFighter(entry) == null || GetOneFighter(entry) == fighter).ToArray();

            var random = new Random();

            if (availableCells.Length == 0 && placement)
            {
                cell = null;
                return false;
            }

            // if not in placement phase, get a random free cell on the map
            if (availableCells.Length == 0 && !placement)
            {
                var cells = Enumerable.Range(0, (int) MapPoint.MapSize).ToList();
                foreach (var actor in GetAllFighters(actor => cells.Contains(actor.Cell.Id)))
                {
                    cells.Remove(actor.Cell.Id);
                }

                cell = Map.Cells[cells[random.Next(cells.Count)]];

                return true;
            }

            cell = availableCells[random.Next(availableCells.Length)];

            return true;
        }


        public bool RandomnizePosition(FightActor fighter)
        {
            if (State != FightState.Placement)
                throw new Exception("State != Placement, cannot random placement position");

            Cell cell;
            if (!FindRandomFreeCell(fighter, out cell))
            {
                if (fighter is CharacterFighter)
                    ((CharacterFighter)fighter).LeaveFight(); // no place more than we kick the actor to avoid bugs
                else
                    fighter.Team.RemoveFighter(fighter);
                return false;
            }

            fighter.ChangePrePlacement(cell);
            return true;
        }

        public void RandomnizePositions(FightTeam team)
        {
            if (State != FightState.Placement)
                throw new Exception("State != Placement, cannot random placement position");

            var shuffledCells = team.PlacementCells.Shuffle();
            var enumerator = shuffledCells.GetEnumerator();
            foreach (var fighter in team.GetAllFighters())
            {
                if (enumerator.MoveNext())
                    fighter.ChangePrePlacement(enumerator.Current);
            }
            enumerator.Dispose();
        }

        public DirectionsEnum FindPlacementDirection(FightActor fighter)
        {
            if (State != FightState.Placement)
                throw new Exception("State != Placement, cannot give placement direction");

            var team = fighter.OpposedTeam;

            Tuple<Cell, uint> closerCell = null;
            foreach (var opposant in team.GetAllFighters())
            {
                var point = opposant.Position.Point;

                if (closerCell == null)
                    closerCell = Tuple.Create(opposant.Cell,
                                              fighter.Position.Point.ManhattanDistanceTo(point));
                else
                {
                    if (fighter.Position.Point.ManhattanDistanceTo(point) < closerCell.Item2)
                        closerCell = Tuple.Create(opposant.Cell,
                                                  fighter.Position.Point.ManhattanDistanceTo(point));
                }
            }

            return closerCell == null ? fighter.Position.Direction : fighter.Position.Point.OrientationTo(new MapPoint(closerCell.Item1), false);
        }

        protected virtual bool CanKickFighter(FightActor kicker, FightActor kicked)
        {
            return State == FightState.Placement && kicker.IsTeamLeader() && kicked.Team == kicker.Team;
        }

        public bool KickFighter(FightActor kicker, FightActor fighter)
        {
            if (!Fighters.Contains(fighter))
                return false;

            if (!CanKickFighter(kicker, fighter))
                return false;

            fighter.Team.RemoveFighter(fighter);

            var characterFighter = fighter as CharacterFighter;
            if (characterFighter != null)
            {
                characterFighter.Character.RejoinMap();
            }

            CheckFightEnd();

            return true;
        }

        /// <summary>
        ///   Set the ready state of a character
        /// </summary>
        protected virtual void OnSetReady(FightActor fighter, bool isReady)
        {
            if (State != FightState.Placement)
                return;

            ContextHandler.SendGameFightHumanReadyStateMessage(Clients, fighter);

            if (ChallengersTeam.AreAllReady() && DefendersTeam.AreAllReady())
                StartFighting();
        }


        /// <summary>
        ///   Check if a character can change position (before the fight is started).
        /// </summary>
        /// <param name = "fighter"></param>
        /// <param name="cell"></param>
        /// <returns>If change is possible</returns>
        public virtual bool CanChangePosition(FightActor fighter, Cell cell)
        {
            var figtherOnCell = GetOneFighter(cell);

            return State == FightState.Placement &&
                   fighter.Team.PlacementCells.Contains(cell) &&
                   (figtherOnCell == fighter || figtherOnCell == null);
        }

        protected virtual void OnSwapPreplacementPosition(FightActor fighter, FightActor actor)
        {
            UpdateFightersPlacementDirection();
            ContextRoleplayHandler.SendGameFightPlacementSwapPositionsMessage(Clients, new[] { fighter, actor });
        }

        protected virtual void OnChangePreplacementPosition(FightActor fighter, ObjectPosition objectPosition)
        {
            UpdateFightersPlacementDirection();
            ContextHandler.SendGameEntitiesDispositionMessage(Clients, GetAllFighters());
        }

        protected void UpdateFightersPlacementDirection()
        {
            foreach (FightActor fighter in Fighters)
            {
                fighter.Position.Direction = FindPlacementDirection(fighter);
            }
        }

        #endregion

        #region Kick



        #endregion

        #endregion

        #endregion

        #region Add/Remove Fighter

        protected virtual void OnFighterAdded(FightTeam team, FightActor actor)
        {
            if (State == FightState.Ended)
            {
                return;
            }

            if (actor is SummonedFighter)
            {
                OnSummonAdded(actor as SummonedFighter);
                return;
            }

            if (actor is SummonedBomb)
            {
                OnBombAdded(actor as SummonedBomb);
                return;
            }

            if (actor is SlaveFighter)
            {
                OnSlaveAdded(actor as SlaveFighter);
                return;
            }

            TimeLine.Fighters.Add(actor);

            BindFighterEvents(actor);

            if (State == FightState.Placement)
            {
                if (!RandomnizePosition(actor))
                    return;
            }

            if (actor is CharacterFighter)
                OnCharacterAdded(actor as CharacterFighter);

            ForEach(entry => ContextHandler.SendGameFightShowFighterMessage(entry.Client, actor), true);

            UpdateBlades(team);
            
            ContextHandler.SendGameFightTurnListMessage(Clients, this);
        }

        protected virtual void OnSummonAdded(SummonedFighter fighter)
        {
            TimeLine.InsertFighter(fighter, TimeLine.Fighters.IndexOf(fighter.Summoner) + 1);
            BindFighterEvents(fighter);

            ContextHandler.SendGameFightTurnListMessage(Clients, this);
        }

        protected virtual void OnSlaveAdded(SlaveFighter slave)
        {
            TimeLine.InsertFighter(slave, TimeLine.Fighters.IndexOf(slave.Summoner) + 1);
            BindFighterEvents(slave);

            ContextHandler.SendGameFightTurnListMessage(Clients, this);
        }

        protected virtual void OnBombAdded(SummonedBomb bomb)
        {
            TimeLine.InsertFighter(bomb, TimeLine.Fighters.IndexOf(bomb.Summoner) + 1);
            BindFighterEvents(bomb);

            ContextHandler.SendGameFightTurnListMessage(Clients, this);
        }

        protected virtual void OnCharacterAdded(CharacterFighter fighter)
        {
            var character = fighter.Character;

            character.RefreshActor();

            if (character.ArenaPopup != null)
                character.ArenaPopup.Deny();

            Clients.Add(character.Client);

            SendGameFightJoinMessage(fighter);

            if (State == FightState.Placement || State == FightState.NotStarted)
            {
                ContextHandler.SendGameFightPlacementPossiblePositionsMessage(character.Client, this, (sbyte)fighter.Team.Id);
            }

            foreach (var fightMember in GetAllFighters())
                ContextHandler.SendGameFightShowFighterMessage(character.Client, fightMember);

            ContextHandler.SendGameEntitiesDispositionMessage(character.Client, GetAllFighters());

            ContextHandler.SendGameFightUpdateTeamMessage(character.Client, this, ChallengersTeam);
            ContextHandler.SendGameFightUpdateTeamMessage(character.Client, this, DefendersTeam);

            ContextHandler.SendGameFightUpdateTeamMessage(Clients, this, fighter.Team);
        }


        protected virtual void OnFighterRemoved(FightTeam team, FightActor actor)
        {
            if (actor is SummonedFighter)
            {
                OnSummonRemoved(actor as SummonedFighter);
                return;
            }

            if (actor is SummonedBomb)
            {
                OnBombRemoved(actor as SummonedBomb);
                return;
            }

            TimeLine.RemoveFighter(actor);
            UnBindFighterEvents(actor);

            if (actor is CharacterFighter)
                OnCharacterRemoved(actor as CharacterFighter);

            switch (State)
            {
                case FightState.Placement:
                    ContextHandler.SendGameFightRemoveTeamMemberMessage(Clients, actor);
                    ContextHandler.SendGameFightRemoveTeamMemberMessage(Map.Clients, actor);
                    break;
                case FightState.Fighting:
                    ContextHandler.SendGameContextRemoveElementMessage(Clients, actor);
                    break;
            }

            UpdateBlades(team);
        }

        protected virtual void OnSummonRemoved(SummonedFighter fighter)
        {
            TimeLine.RemoveFighter(fighter);
            UnBindFighterEvents(fighter);

            ContextHandler.SendGameFightTurnListMessage(Clients, this);
        }
        protected virtual void OnBombRemoved(SummonedBomb bomb)
        {
            TimeLine.RemoveFighter(bomb);
            UnBindFighterEvents(bomb);

            ContextHandler.SendGameFightTurnListMessage(Clients, this);
        }

        protected virtual void OnCharacterRemoved(CharacterFighter fighter)
        {
            Clients.Remove(fighter.Character.Client);
        }


        #endregion

        #region Spectators

        public void ToggleSpectatorClosed(Character character, bool state)
        {
            SpectatorClosed = state;

            // Spectator mode Activated/Disabled
            BasicHandler.SendTextInformationMessage(Clients, TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, (short)( SpectatorClosed ? 40 : 39 ), character.Name);

            if (state)
                RemoveAllSpectators();

            OnTeamOptionsChanged(ChallengersTeam, FightOptionsEnum.FIGHT_OPTION_SET_SECRET);
            OnTeamOptionsChanged(DefendersTeam, FightOptionsEnum.FIGHT_OPTION_SET_SECRET);
        }

        public virtual bool CanSpectatorJoin(Character spectator) => !SpectatorClosed && (State == FightState.Placement || State == FightState.Fighting);

        public bool AddSpectator(FightSpectator spectator)
        {
            if (!CanSpectatorJoin(spectator.Character))
                return false;

            m_spectators.Add(spectator);
            spectator.JoinTime = DateTime.Now;
            spectator.Left += OnSpectectorLeft;
            spectator.Character.LoggedOut += OnSpectatorLoggedOut;

            Clients.Add(spectator.Client);
            SpectatorClients.Add(spectator.Client);

            OnSpectatorAdded(spectator);

            return true;
        }

        protected virtual void OnSpectatorAdded(FightSpectator spectator)
        {
            SendGameFightSpectatorJoinMessage(spectator);

            if (State == FightState.Placement || State == FightState.NotStarted)
                ContextHandler.SendGameFightPlacementPossiblePositionsMessage(spectator.Client, this, 0);

            foreach (var fighter in GetAllFighters())
                ContextHandler.SendGameFightShowFighterMessage(spectator.Client, fighter);

            ContextHandler.SendGameFightTurnListMessage(spectator.Client, this);
            ContextHandler.SendGameFightSpectateMessage(spectator.Client, this);
            ContextHandler.SendGameFightNewRoundMessage(spectator.Client, TimeLine.RoundNumber);

            CharacterHandler.SendCharacterStatsListMessage(spectator.Client);

            if (Challenge != null)
            {
                ContextHandler.SendChallengeInfoMessage(spectator.Client, Challenge);
                if (Challenge.Status != ChallengeStatusEnum.RUNNING)
                    ContextHandler.SendChallengeResultMessage(spectator.Client, Challenge);
            }
            // Spectator 'X' joined
            BasicHandler.SendTextInformationMessage(Clients, TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 36, spectator.Character.Name);

            if (TimeLine.Current != null)
                ContextHandler.SendGameFightTurnResumeMessage(spectator.Client, FighterPlaying);
        }

        protected virtual void OnSpectatorLoggedOut(Character character)
        {
            if (!character.IsSpectator())
                return;

            OnSpectectorLeft(character.Spectator);
        }

        protected virtual void OnSpectectorLeft(FightSpectator spectator)
        {
            RemoveSpectator(spectator);
        }

        public void RemoveSpectator(FightSpectator spectator)
        {
            m_spectators.Remove(spectator);

            Clients.Remove(spectator.Character.Client);
            SpectatorClients.Remove(spectator.Client);

            spectator.Left -= OnSpectectorLeft;
            spectator.Character.LoggedOut -= OnSpectatorLoggedOut;

            OnSpectatorRemoved(spectator);
        }

        protected virtual void OnSpectatorRemoved(FightSpectator spectator)
        {
            spectator.Character.RejoinMap();
        }

        public void RemoveAllSpectators()
        {
            foreach (var spectator in m_spectators.GetRange(0, Spectators.Count))
            {
                RemoveSpectator(spectator);
            }
        }

        #endregion

        #region Turn Management

        public void StartTurn()
        {
            if (State != FightState.Fighting)
                return;

            if (!CheckFightEnd())
            {
                OnTurnStarted();
            }
        }

        public event Action<IFight, FightActor> TurnStarted;

        protected virtual void OnTurnStarted()
        {
            if (TimeLine.NewRound)
            {
                ContextHandler.SendGameFightNewRoundMessage(Clients, TimeLine.RoundNumber);
            }

            ContextHandler.SendGameFightTurnStartMessage(Clients, FighterPlaying.Id, FighterPlaying.TurnTime / 100);

            StartSequence(SequenceTypeEnum.SEQUENCE_TURN_START);

            if (!FighterPlaying.IsSummoned())
            {
                FighterPlaying.DecrementAllCastedBuffsDuration();
                FighterPlaying.DecrementSummonsCastedBuffsDuration();
            }

            foreach(var passedFighter in TimeLine.PassedActors)
            {
                passedFighter.DecrementAllCastedBuffsDuration();
                passedFighter.DecrementSummonsCastedBuffsDuration();
            }

            DecrementGlyphDuration(FighterPlaying);
            FighterPlaying.TriggerBuffs(FighterPlaying, BuffTriggerType.OnTurnBegin);
            TriggerMarks(FighterPlaying.Cell, FighterPlaying, TriggerType.OnTurnBegin);

            EndSequence(SequenceTypeEnum.SEQUENCE_TURN_START);

            // can die with triggers
            if (CheckFightEnd())
                return;

            if (FighterPlaying.MustSkipTurn())
            {
                FighterPlaying.ResetUsedPoints();
                PassTurn();

                return;
            }

            ForEach(entry => ContextHandler.SendGameFightSynchronizeMessage(entry.Client, this), true);
            ForEach(entry => entry.RefreshStats());

            if (FighterPlaying is CharacterFighter)
            {
                var characterFighter = FighterPlaying as CharacterFighter;

                ContextHandler.SendGameFightTurnStartPlayingMessage(characterFighter.Character.Client);
                ContextHandler.SendFighterStatsListMessage(characterFighter.Character.Client, characterFighter.Character);
            }
            else if (FighterPlaying is SlaveFighter)
            {
                var slaveFighter = FighterPlaying as SlaveFighter;
                var characterFighter = slaveFighter.Summoner as CharacterFighter;

                ContextHandler.SendGameFightTurnStartPlayingMessage(characterFighter.Character.Client);
            }

            FighterPlaying.TurnStartPosition = FighterPlaying.Position.Clone();

            TurnStartTime = DateTime.Now;

            if (!Freezed)
            {
                if (!Map.Area.IsRunning)
                    Map.Area.Start();

                m_turnTimer = Map.Area.CallDelayed(FighterPlaying.TurnTime, StopTurn);
            }
                

            var evnt = TurnStarted;
            if (evnt != null)
                evnt(this, FighterPlaying);
        }

        public void StopTurn()
        {
            if (State != FightState.Fighting)
                return;

            if (m_turnTimer != null)
                m_turnTimer.Dispose();

            if (ReadyChecker != null)
            {
                logger.Debug("Last ReadyChecker was not disposed. (Stop Turn)");
                ReadyChecker.Cancel();
                ReadyChecker = null;
            }
            
            if (CheckFightEnd())
                return;

            OnTurnStopped();
            ReadyChecker = ReadyChecker.RequestCheck(this, PassTurnAndCheck, LagAndPassTurn);
        }

        public event Action<IFight, FightActor> BeforeTurnStopped;
        public event Action<IFight, FightActor> TurnStopped;

        protected virtual void OnTurnStopped()
        {
            var evnt = BeforeTurnStopped;
            if (evnt != null)
                evnt(this, FighterPlaying);

            StartSequence(SequenceTypeEnum.SEQUENCE_TURN_END);

            if (FighterPlaying.IsAlive())
            {
                FighterPlaying.TriggerBuffs(FighterPlaying, BuffTriggerType.OnTurnEnd);
                FighterPlaying.TriggerBuffsRemovedOnTurnEnd();
                TriggerMarks(FighterPlaying.Cell, FighterPlaying, TriggerType.OnTurnEnd);
            }

            var time = (int)Math.Floor(GetTurnTimeLeft().TotalSeconds / 2);
            if (TimeLine.RoundNumber > 1)
                FighterPlaying.TurnTimeReport = time > 0 ? time : 0;

            EndSequence(SequenceTypeEnum.SEQUENCE_TURN_END);

            // can die with triggers
            if (CheckFightEnd())
                return;

            if (IsSequencing)
                EndSequence(Sequence, true);

            if (WaitAcknowledgment)
                AcknowledgeAction();

            evnt = TurnStopped;
            if (evnt != null)
                evnt(this, FighterPlaying);

            ContextHandler.SendGameFightTurnEndMessage(Clients, FighterPlaying);
        }

        protected void LagAndPassTurn(NamedFighter[] laggers)
        {
            if (ReadyChecker == null)
                return;

            // some guys are lagging !
            OnLaggersSpotted(laggers);

            PassTurnAndCheck();
        }

        protected void PassTurnAndCheck()
        {
            if (ReadyChecker == null)
                return;

            ReadyChecker = null;

            FighterPlaying.ResetUsedPoints();
            PassTurn();
        }

        protected void PassTurn()
        {
            if (State != FightState.Fighting)
                return;

            if (CheckFightEnd())
                return;

            redo:
            if (!TimeLine.SelectNextFighter())
            {
                if (!CheckFightEnd())
                {
                    logger.Error("Something goes wrong : no more actors are available to play but the fight is not ended");
                }

                return;
            }

            // player left but is disconnected
            // pass turn is there are others players
            if (FighterPlaying.HasLeft() && FighterPlaying is CharacterFighter)
            {

                var leaver = (CharacterFighter) FighterPlaying;
                if (leaver.IsDisconnected &&
                    leaver.LeftRound + FightConfiguration.TurnsBeforeDisconnection <= TimeLine.RoundNumber)
                {
                    leaver.Die();

                    if (CheckFightEnd())
                        return;

                    IFightResult leaverResult;
                    var results = GenerateLeaverResults(leaver, out leaverResult);

                    leaverResult.Apply();

                    ContextHandler.SendGameFightLeaveMessage(Clients, leaver);

                    leaver.ResetFightProperties();

                    leaver.Team.AddLeaver(leaver);
                    m_leavers.Add(leaver);
                    leaver.Team.RemoveFighter(leaver);

                    leaver.LeaveDisconnectedState(false);

                    leaver.Character.RejoinMap();
                    leaver.Character.SaveLater();
 
                    goto redo;
                }

                // <b>%1</b> vient d'être déconnecté, il quittera la partie dans <b>%2</b> tour(s) s'il ne se reconnecte pas avant.
                BasicHandler.SendTextInformationMessage(Clients, TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 182,
                    FighterPlaying.GetMapRunningFighterName(), leaver.LeftRound + FightConfiguration.TurnsBeforeDisconnection - TimeLine.RoundNumber);
            }


            OnTurnPassed();

            StartTurn();
        }

        protected virtual void OnTurnPassed()
        {
            if (IsSequencing)
                EndSequence(Sequence, true);

            if (WaitAcknowledgment)
                AcknowledgeAction();
        }

        #endregion

        #region Events Binders

        private void UnBindFightersEvents()
        {
            foreach (var fighter in Fighters)
            {
                UnBindFighterEvents(fighter);
            }
        }

        void UnBindFighterEvents(FightActor actor)
        {
            actor.ReadyStateChanged -= OnSetReady;
            actor.PrePlacementChanged -= OnChangePreplacementPosition;
            actor.PrePlacementSwapped -= OnSwapPreplacementPosition;
            actor.FighterLeft -= OnPlayerLeft;

            actor.StartMoving -= OnStartMoving;
            actor.StopMoving -= OnStopMoving;
            actor.PositionChanged -= OnPositionChanged;
            actor.FightPointsVariation -= OnFightPointsVariation;
            actor.LifePointsChanged -= OnLifePointsChanged;
            actor.DamageReducted -= OnDamageReducted;
            actor.SpellCasting -= OnSpellCasting;
            actor.SpellCasted -= OnSpellCasted;
            actor.WeaponUsed -= OnCloseCombat;
            actor.BuffAdded -= OnBuffAdded;
            actor.BuffRemoved -= OnBuffRemoved;
            actor.Dead -= OnDead;


            var fighter = actor as CharacterFighter;

            if (fighter != null)
            {
                fighter.Character.LoggedOut -= OnPlayerLoggout;
                fighter.Character.StatsResfreshed -= OnStatsRefreshed;
            }
        }

        private void BindFightersEvents()
        {
            foreach (var fighter in Fighters)
            {
                BindFighterEvents(fighter);
            }
        }

        private void BindFighterEvents(FightActor actor)
        {
            if (State == FightState.Placement)
            {
                actor.FighterLeft += OnPlayerLeft;

                actor.ReadyStateChanged += OnSetReady;
                actor.PrePlacementChanged += OnChangePreplacementPosition;
                actor.PrePlacementSwapped += OnSwapPreplacementPosition;
            }

            if (State == FightState.Fighting)
            {
                actor.FighterLeft += OnPlayerLeft;

                actor.StartMoving += OnStartMoving;
                actor.StopMoving += OnStopMoving;
                actor.PositionChanged += OnPositionChanged;
                actor.FightPointsVariation += OnFightPointsVariation;
                actor.LifePointsChanged += OnLifePointsChanged;
                actor.DamageReducted += OnDamageReducted;

                actor.SpellCasting += OnSpellCasting;
                actor.SpellCasted += OnSpellCasted;
                actor.SpellCastFailed += OnSpellCastFailed;
                actor.WeaponUsed += OnCloseCombat;

                actor.BuffAdded += OnBuffAdded;
                actor.BuffRemoved += OnBuffRemoved;

                actor.Dead += OnDead;
            }

            var fighter = actor as CharacterFighter;

            if (fighter != null)
            {
                fighter.Character.LoggedOut += OnPlayerLoggout;

                if (State == FightState.Placement)
                {
                    fighter.Character.StatsResfreshed += OnStatsRefreshed;
                }
            }
        }

        #endregion

        #region Turn Actions

        #region Death

        protected virtual void OnDead(FightActor fighter, FightActor killedBy)
        {
            StartSequence(SequenceTypeEnum.SEQUENCE_CHARACTER_DEATH);

            ActionsHandler.SendGameActionFightDeathMessage(Clients, fighter);

            fighter.KillAllSummons();
            fighter.RemoveAndDispellAllBuffs(FightDispellableEnum.DISPELLABLE_BY_DEATH);
            fighter.RemoveAllCastedBuffs(FightDispellableEnum.DISPELLABLE_BY_DEATH);

            EndSequence(SequenceTypeEnum.SEQUENCE_CHARACTER_DEATH);

            foreach (var trigger in m_triggers.Where(trigger => trigger.Caster == fighter).ToArray())
            {
                RemoveTrigger(trigger);
            }
        }

        #endregion

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

            StartSequence(SequenceTypeEnum.SEQUENCE_MOVE);
            
            int index = 0;
            foreach(var tackle in fightPath.Tackles)
            {
                if (tackle.PathIndex - index > 0)
                {
                    ForEach(entry =>
                    {
                        if (entry.CanSee(fighter))
                            ContextHandler.SendGameMapMovementMessage(entry.Client, fightPath.Cells.Skip(index).Take(tackle.PathIndex - index + 1).Select(x => x.Id), fighter);
                    }, true);
                }
                OnTackled(fighter, tackle);
                index = tackle.PathIndex;
            }


            if (path.Cells.Length - index > 0)
            {
                ForEach(entry =>
                {
                    if (entry.CanSee(fighter))
                        ContextHandler.SendGameMapMovementMessage(entry.Client, fightPath.Cells.Skip(index).Take(path.Cells.Length - index + 1).Select(x => x.Id), fighter);
                }, true);
            }

            if (fightPath.BlockedByObstacle)
            {
                // "Impossible d'emprunter ce chemin : un obstacle bloque le passage !"
                character?.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 276);
            }

            actor.StopMove();
            EndSequence(SequenceTypeEnum.SEQUENCE_MOVE);
        }

        public event Action<FightActor, int, int> Tackled;

        protected virtual void OnTackled(FightActor actor, FightTackle tackle)
        {
            if (actor.MP - tackle.TackledMP < 0)
            {
                logger.Error("Cannot apply tackle : mp tackled ({0}) > available mp ({1})", tackle.TackledMP, actor.MP);
                return;
            }

            ActionsHandler.SendGameActionFightTackledMessage(Clients, actor, tackle.Tacklers);
            actor.LostAP((short)tackle.TackledAP, actor);
            actor.LostMP((short)tackle.TackledMP, actor);
            
            actor.TriggerBuffs(actor, BuffTriggerType.OnTackled);

            foreach (var tackler in tackle.Tacklers)
                tackler.TriggerBuffs(tackler, BuffTriggerType.OnTackle);

            var handler = Tackled;
            if (handler != null)
                handler(actor, tackle.TackledAP, tackle.TackledMP);
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

            fighter.UseMP((short) path.MPCost);
        }

        protected virtual void OnPositionChanged(ContextActor actor, ObjectPosition objectPosition)
        {
            var fighter = actor as FightActor;

            if (fighter == null)
                return;

            TriggerMarks(fighter.Cell, fighter, TriggerType.MOVE);
        }

        #endregion

        #region Health & Actions points

        protected virtual void OnLifePointsChanged(FightActor actor, int delta, int shieldDamages, int permanentDamages, FightActor from, EffectSchoolEnum school)
        {
            var loss = (short) (-delta);

            if (delta == 0 && shieldDamages == 0 && permanentDamages == 0)
                return;

            var action = ActionsEnum.ACTION_CHARACTER_LIFE_POINTS_LOST;

            switch (school)
            {
                case EffectSchoolEnum.Air:
                    action = ActionsEnum.ACTION_CHARACTER_LIFE_POINTS_LOST_FROM_AIR;
                    break;
                case EffectSchoolEnum.Earth:
                    action = ActionsEnum.ACTION_CHARACTER_LIFE_POINTS_LOST_FROM_EARTH;
                    break;
                case EffectSchoolEnum.Fire:
                    action = ActionsEnum.ACTION_CHARACTER_LIFE_POINTS_LOST_FROM_FIRE;
                    break;
                case EffectSchoolEnum.Water:
                    action = ActionsEnum.ACTION_CHARACTER_LIFE_POINTS_LOST_FROM_WATER;
                    break;
                case EffectSchoolEnum.Pushback:
                    action = ActionsEnum.ACTION_CHARACTER_LIFE_POINTS_LOST_FROM_PUSH;
                    break;
                default:
                    action = ActionsEnum.ACTION_CHARACTER_LIFE_POINTS_LOST;
                    break;
            }

            var shieldBuffs = actor.GetBuffs(x => x is StatBuff && ((StatBuff)x).Caracteristic == PlayerFields.Shield).Select(x => x as StatBuff).ToArray();

            if (!shieldBuffs.Any() && shieldDamages == 0)
                ActionsHandler.SendGameActionFightLifePointsLostMessage(Clients, action, from ?? actor, actor, loss, (short)permanentDamages);
            else
            {
                ActionsHandler.SendGameActionFightLifeAndShieldPointsLostMessage(Clients, action, from ?? actor, actor, loss,
                    (short)permanentDamages, (short)shieldDamages);

                foreach (var shieldBuff in shieldBuffs)
                {
                    if (shieldDamages <= 0)
                        continue;

                    var diff = Math.Max(0, shieldBuff.Value - shieldDamages);

                    shieldDamages -= (shieldBuff.Value - diff);
                    shieldBuff.Value = (short)diff;

                    if (shieldBuff.Value <= 0)
                        actor.RemoveBuff(shieldBuff);
                    else
                        ContextHandler.SendGameActionFightDispellableEffectMessage(Clients, shieldBuff, true);
                }
            }
        }

        protected virtual void OnFightPointsVariation(FightActor actor, ActionsEnum action, FightActor source, FightActor target, short delta)
        {
            if (delta == 0)
                return;

            ActionsHandler.SendGameActionFightPointsVariationMessage(Clients, action, source, target, delta);
        }

        protected virtual void OnDamageReducted(FightActor fighter, FightActor source, int reduction)
        {
            if (reduction == 0)
                return;

            ActionsHandler.SendGameActionFightReduceDamagesMessage(Clients, source, fighter, reduction);
        }

        #endregion

        #region Spells

        protected virtual void OnCloseCombat(FightActor caster, WeaponTemplate weapon, Cell targetCell, FightSpellCastCriticalEnum critical, bool silentCast)
        {
            var target = GetOneFighter(targetCell);
            ForEach(entry => ActionsHandler.SendGameActionFightCloseCombatMessage(entry.Client, caster, target, targetCell, critical,
                !caster.IsVisibleFor(entry) || silentCast, weapon), true);
        }


        protected virtual void OnSpellCasting(FightActor caster, Spell spell, Cell targetCell, FightSpellCastCriticalEnum critical, bool silentCast)
        {
            var target = GetOneFighter(targetCell);

            if (spell.Id == 0)
            {
                ForEach(entry => ActionsHandler.SendGameActionFightCloseCombatMessage(entry.Client, caster, target, targetCell, critical,
                                                                                !caster.IsVisibleFor(entry) || silentCast, 0), true);
            }
            else
                ForEach(entry => ContextHandler.SendGameActionFightSpellCastMessage(entry.Client, ActionsEnum.ACTION_FIGHT_CAST_SPELL,
                                                                                caster, target, targetCell, critical, !caster.IsVisibleFor(entry) || silentCast, spell), true);
        }

        protected virtual void OnSpellCasted(FightActor caster, Spell spell, Cell target, FightSpellCastCriticalEnum critical, bool silentCast)
        {
            EndSequence(SequenceTypeEnum.SEQUENCE_SPELL);

            CheckFightEnd();
        }

        protected virtual void OnSpellCastFailed(FightActor caster, Spell spell, Cell target)
        {
            ContextHandler.SendGameActionFightNoSpellCastMessage(Clients, spell);
        }

        #endregion

        #region Buffs

        public IEnumerable<Buff> GetBuffs()
        {
            return m_buffs;
        }

        public void UpdateBuff(Buff buff, bool updateAction = true)
        {
            ContextHandler.SendGameActionFightDispellableEffectMessage(Clients, buff, updateAction);
        }

        protected virtual void OnBuffAdded(FightActor target, Buff buff)
        {
            m_buffs.Add(buff);
            ContextHandler.SendGameActionFightDispellableEffectMessage(Clients, buff);
        }

        protected virtual void OnBuffRemoved(FightActor target, Buff buff)
        {
            m_buffs.Remove(buff);

            // regular debuffing is done automatically
            ActionsHandler.SendGameActionFightDispellEffectMessage(Clients, target, target, buff);
        }

        #endregion

        #region Sequences

        private SequenceTypeEnum m_lastSequenceAction;
        private int m_sequenceLevel;
        private readonly Stack<SequenceTypeEnum> m_sequences = new Stack<SequenceTypeEnum>(); 

        public SequenceTypeEnum Sequence
        {
            get;
            private set;
        }

        public bool IsSequencing
        {
            get;
            private set;
        }

        public bool WaitAcknowledgment
        {
            get;
            private set;
        }

        public bool StartSequence(SequenceTypeEnum sequenceType)
        {
            // even if a sequence is already running we just increase the level variable and notify the last action
            m_lastSequenceAction = sequenceType;
            m_sequenceLevel++;

            if (IsSequencing)
                return false;

            IsSequencing = true;
            Sequence = sequenceType;
            m_sequences.Push(sequenceType);

            ActionsHandler.SendSequenceStartMessage(Clients, TimeLine.Current, sequenceType);

            return true;
        }


        public bool EndSequence(SequenceTypeEnum sequenceType, bool force = false)
        {
            if (!IsSequencing)
                return false;

            m_sequenceLevel--;

            if (m_sequenceLevel > 0 && !force)
                return false;

            IsSequencing = false;
            WaitAcknowledgment = true;

            var poppedSequence = m_sequences.Pop();

            if (poppedSequence != sequenceType)
            {
                logger.Debug("Popped Sequence different ({0} != {1})", poppedSequence, sequenceType);
            }

            ActionsHandler.SendSequenceEndMessage(Clients, TimeLine.Current, sequenceType, m_lastSequenceAction);

            return true;
        }

        public void EndAllSequences()
        {
            m_sequenceLevel = 0;
            IsSequencing = false;
            WaitAcknowledgment = false;

            while (m_sequences.Count > 0)
            {
                var poppedSequence = m_sequences.Pop();

                ActionsHandler.SendSequenceEndMessage(Clients, TimeLine.Current, poppedSequence, m_lastSequenceAction);
            }
        }

        public virtual void AcknowledgeAction()
        {
            WaitAcknowledgment = false;

            // todo : find the right usage
        }

        #endregion


        #endregion

        #region Non Turn Actions

        protected virtual void OnPlayerLeft(FightActor fighter)
        {
            if (!(fighter is CharacterFighter))
            {
                logger.Error("Only characters can leave a fight");
                return;
            }

            var characterFighter = ((CharacterFighter)fighter);

            if (characterFighter.Character.IsLoggedIn)
            {
                if (State == FightState.Placement)
                {
                    characterFighter.ResetFightProperties();

                    if (CheckFightEnd())
                        return;

                    fighter.Team.RemoveFighter(fighter);
                    characterFighter.Character.RejoinMap();
                }
                else
                {
                    fighter.Die();

                    // wait the character to be ready
                    var readyChecker = new ReadyChecker(this, new[] { characterFighter });
                    readyChecker.Success += obj => OnPlayerReadyToLeave(characterFighter);
                    readyChecker.Timeout += (obj, laggers) => OnPlayerReadyToLeave(characterFighter);

                    characterFighter.PersonalReadyChecker = readyChecker;
                    readyChecker.Start();
                }
            }
            else
            {
                var isfighterTurn = fighter.IsFighterTurn();
                characterFighter.EnterDisconnectedState();

                if (!CheckFightEnd() && isfighterTurn && characterFighter.MustSkipTurn())
                    StopTurn();

                fighter.Team.AddLeaver(fighter);
                m_leavers.Add(fighter);

                // <b>%1</b> vient d'être déconnecté, il quittera la partie dans <b>%2</b> tour(s) s'il ne se reconnecte pas avant.
                BasicHandler.SendTextInformationMessage(Clients, TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 182,
                    fighter.GetMapRunningFighterName(), FightConfiguration.TurnsBeforeDisconnection);
            }
        }

        protected virtual void OnPlayerReadyToLeave(CharacterFighter fighter)
        {
            if (fighter.Fight != fighter.Character.Fight)
                return;


            fighter.PersonalReadyChecker = null;
            var isfighterTurn = fighter.IsFighterTurn();

            IFightResult leaverResult;
            var results = GenerateLeaverResults(fighter, out leaverResult);

            leaverResult.Apply();

            ContextHandler.SendGameFightLeaveMessage(Clients, fighter);
            ContextHandler.SendGameFightEndMessage(fighter.Character.Client, this,
                results.Select(x => x.GetFightResultListEntry()));

            var fightend = CheckFightEnd();

            if (!fightend && isfighterTurn)
                StopTurn();

            fighter.ResetFightProperties();

            fighter.Team.AddLeaver(fighter);
            m_leavers.Add(fighter);

            fighter.Team.RemoveFighter(fighter);
            fighter.Character.RejoinMap();
        }

        protected virtual void OnPlayerLoggout(Character character)
        {
            character.LoggedOut -= OnPlayerLoggout;
            if (!character.IsFighting() || character.Fight != this)
                return;

            character.Fighter.LeaveFight();
        }

        public void RejoinFightFromDisconnection(CharacterFighter fighter)
        {
            fighter.Character.LoggedOut += OnPlayerLoggout;
            fighter.Team.RemoveLeaver(fighter);
            m_leavers.Remove(fighter);
            fighter.LeaveDisconnectedState();

            var client = fighter.Character.Client;

            Clients.Add(client);

            SendGameFightJoinMessage(fighter);
            
            foreach (var fightMember in GetAllFighters())
                ContextHandler.SendGameFightShowFighterMessage(client, fightMember);

            fighter.Character.RefreshStats();

            if (State == FightState.Placement || State == FightState.NotStarted)
                ContextHandler.SendGameFightPlacementPossiblePositionsMessage(client, this, (sbyte)fighter.Team.Id);

            ContextHandler.SendGameEntitiesDispositionMessage(client, GetAllFighters());
            ContextHandler.SendGameFightResumeMessage(client, fighter);
            ContextHandler.SendGameFightTurnListMessage(client, this);
            ContextHandler.SendGameFightSynchronizeMessage(client, this);

            if (fighter.IsSlaveTurn())
                ContextHandler.SendSlaveSwitchContextMessage(client, fighter.GetSlave());

            ContextHandler.SendGameFightTurnResumeMessage(client, FighterPlaying);
            ContextHandler.SendGameFightNewRoundMessage(client, TimeLine.RoundNumber);
            ContextHandler.SendGameFightUpdateTeamMessage(client, this, ChallengersTeam);
            ContextHandler.SendGameFightUpdateTeamMessage(client, this, DefendersTeam);

            ContextHandler.SendGameFightOptionStateUpdateMessage(client, fighter.Team, FightOptionsEnum.FIGHT_OPTION_ASK_FOR_HELP, fighter.Team.GetOptionState(FightOptionsEnum.FIGHT_OPTION_ASK_FOR_HELP));
            ContextHandler.SendGameFightOptionStateUpdateMessage(client, fighter.Team, FightOptionsEnum.FIGHT_OPTION_SET_CLOSED, fighter.Team.GetOptionState(FightOptionsEnum.FIGHT_OPTION_SET_CLOSED));
            ContextHandler.SendGameFightOptionStateUpdateMessage(client, fighter.Team, FightOptionsEnum.FIGHT_OPTION_SET_SECRET, fighter.Team.GetOptionState(FightOptionsEnum.FIGHT_OPTION_SET_SECRET));
            ContextHandler.SendGameFightOptionStateUpdateMessage(client, fighter.Team, FightOptionsEnum.FIGHT_OPTION_SET_TO_PARTY_ONLY, fighter.Team.GetOptionState(FightOptionsEnum.FIGHT_OPTION_SET_TO_PARTY_ONLY));

            // <b>%1</b> vient de se reconnecter en combat.
            BasicHandler.SendTextInformationMessage(Clients, TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 184, fighter.GetMapRunningFighterName());
        }

        #endregion

        #region Challenges

        public void SetChallenge(DefaultChallenge challenge)
        {
            if (Challenge != null)
                return;
            
            Challenge = challenge;
            ContextHandler.SendChallengeInfoMessage(Clients, challenge);
        }

        public int GetChallengeBonus()
        {
            if (Challenge == null)
                return 0;

            return Challenge.Status == ChallengeStatusEnum.SUCCESS ? Challenge.Bonus : 1;
        }

        #endregion

        #region Triggers

        readonly List<MarkTrigger> m_triggers = new List<MarkTrigger>();

        public IEnumerable<MarkTrigger> GetTriggers() => m_triggers;

        public bool ShouldTriggerOnMove(Cell cell, FightActor actor)
            => m_triggers.Any(entry => entry.TriggerType.HasFlag(TriggerType.MOVE) && entry.StopMovement && entry.ContainsCell(cell) && entry.CanTrigger(actor));

        public MarkTrigger[] GetTriggersByCell(Cell cell) => m_triggers.Where(entry => entry.ContainsCell(cell)).ToArray();

        public MarkTrigger[] GetTriggers(Cell cell) => m_triggers.Where(entry => entry.CenterCell.Id == cell.Id).ToArray();

        public void AddTriger(MarkTrigger trigger)
        {
            trigger.Triggered += OnMarkTriggered;
            m_triggers.Add(trigger);

            foreach (var character in GetCharactersAndSpectators())
            {
                ContextHandler.SendGameActionFightMarkCellsMessage(character.Client, trigger, character.Fighter != null && trigger.DoesSeeTrigger(character.Fighter));
            }

            if (!trigger.TriggerType.HasFlag(TriggerType.CREATION))
                return;

            var fighters = GetAllFighters(trigger.GetCells());
            foreach (var fighter in fighters)
                trigger.Trigger(fighter);
        }

        public void RemoveTrigger(MarkTrigger trigger)
        {
            trigger.Triggered -= OnMarkTriggered;
            m_triggers.Remove(trigger);
            trigger.NotifyRemoved();

            ContextHandler.SendGameActionFightUnmarkCellsMessage(Clients, trigger);
        }

        public void TriggerMarks(Cell cell, FightActor trigger, TriggerType triggerType)
        {
            var triggers = m_triggers.ToArray();
            
            // we use a copy 'cause a trigger can be deleted when a fighter die with it
            foreach (var markTrigger in triggers.Where(markTrigger => markTrigger.TriggerType.HasFlag(triggerType) && markTrigger.ContainsCell(cell)).OrderByDescending(x => x.Priority))
            {
                if (!trigger.CanPlay() && (triggerType == TriggerType.OnTurnBegin || triggerType == TriggerType.OnTurnEnd)
                    && (markTrigger is Wall || markTrigger is Glyph))
                    continue;

                StartSequence(SequenceTypeEnum.SEQUENCE_GLYPH_TRAP);
                markTrigger.Trigger(trigger);
                EndSequence(SequenceTypeEnum.SEQUENCE_GLYPH_TRAP);
            }
        }

        public void DecrementGlyphDuration(FightActor caster)
        {
            var triggersToRemove = m_triggers.Where(trigger => trigger.Caster == caster).
                Where(trigger => trigger.DecrementDuration()).ToList();

            if (triggersToRemove.Count == 0)
                return;

            StartSequence(SequenceTypeEnum.SEQUENCE_GLYPH_TRAP);
            foreach (var trigger in triggersToRemove)
            {
                RemoveTrigger(trigger);
            }
            EndSequence(SequenceTypeEnum.SEQUENCE_GLYPH_TRAP);
        }

        public int PopNextTriggerId() => m_triggerIdProvider.Pop();

        public void FreeTriggerId(int id)
        {
            m_triggerIdProvider.Push(id);
        }

        private void OnMarkTriggered(MarkTrigger markTrigger, FightActor trigger, Spell triggerSpell)
        {
            ContextHandler.SendGameActionFightTriggerGlyphTrapMessage(Clients, markTrigger, trigger, triggerSpell);
        }

        #endregion

        #region Ready Checker

        protected virtual void OnLaggersSpotted(NamedFighter[] laggers)
        {
            if (laggers.Length == 1)
            {
                BasicHandler.SendTextInformationMessage(Clients, TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 28, laggers[0].Name);
            }
            else if (laggers.Length > 1)
            {
                BasicHandler.SendTextInformationMessage(Clients, TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 29, string.Join(",", laggers.Select(entry => entry.Name)));
            }
        }

        #endregion

        #region Freeze

        public bool Freezed
        {
            get { return m_freezed; }
            set { m_freezed = value;
                OnFreezed();
            }
        }

        private void OnFreezed()
        {
            if (State == FightState.Fighting)
            {
                if (Freezed)
                    m_turnTimer.Stop();
                else
                    m_turnTimer = Map.Area.CallDelayed(FightConfiguration.TurnTime, StopTurn);
            }
        }

        #endregion

        #region Send Methods

        protected virtual void SendGameFightJoinMessage(CharacterFighter fighter)
        {
            ContextHandler.SendGameFightJoinMessage(fighter.Character.Client, CanCancelFight(), true, IsStarted, IsStarted ? 0 : (int)GetPlacementTimeLeft().TotalMilliseconds/100, FightType);
        }

        protected virtual void SendGameFightSpectatorJoinMessage(FightSpectator spectator)
        {
            ContextHandler.SendGameFightSpectatorJoinMessage(spectator.Character.Client, false, false, IsStarted, 0, FightType);
        }

        #endregion

        #region Get Methods

        private readonly WorldClientCollection m_clients = new WorldClientCollection();
        private readonly WorldClientCollection m_spectatorClients = new WorldClientCollection();
        private readonly List<FightActor> m_leavers;
        private readonly List<FightSpectator> m_spectators;
        private bool m_freezed;

        /// <summary>
        /// Do not modify, just read
        /// </summary>
        public WorldClientCollection Clients
        {
            get { return m_clients; }
        }

        /// <summary>
        /// Do not modify, just read
        /// </summary>
        public WorldClientCollection SpectatorClients
        {
            get
            {
                return m_spectatorClients;
            }
        }

        public IEnumerable<Character> GetAllCharacters()
        {
            return GetAllCharacters(false);
        }

        public IEnumerable<Character> GetAllCharacters(bool withSpectators = false)
        {
            return withSpectators ? Fighters.OfType<CharacterFighter>().Select(entry => entry.Character).Concat(Spectators.Select(entry => entry.Character)) : Fighters.OfType<CharacterFighter>().Select(entry => entry.Character);
        }

        public void ForEach(Action<Character> action)
        {
            foreach (var character in GetAllCharacters())
            {
                action(character);
            }
        }

        public void ForEach(Action<Character> action, bool withSpectators = false)
        {
            foreach (var character in GetAllCharacters(withSpectators))
            {
                action(character);
            }
        }

        public void ForEach(Action<Character> action, Character except, bool withSpectators = false)
        {
            foreach (var character in GetAllCharacters(withSpectators).Where(character => character != except))
            {
                action(character);
            }
        }

        protected abstract bool CanCancelFight();

        public bool IsCellFree(Cell cell)
        {
            return cell.Walkable && !cell.NonWalkableDuringFight && GetOneFighter(cell) == null;
        }

        public TimeSpan GetFightDuration()
        {
            return TimeSpan.FromMilliseconds(!IsStarted ? 0 : (int) (DateTime.Now - StartTime).TotalMilliseconds);
        }

        public TimeSpan GetTurnTimeLeft()
        {
            if (TimeLine.Current == null)
                return TimeSpan.Zero;

            var time = ( DateTime.Now - TurnStartTime ).TotalMilliseconds;

            return TimeSpan.FromMilliseconds(time > 0 ? (FighterPlaying.TurnTime - (int)time) : 0);
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

        public ReadOnlyCollection<FightActor> GetLeavers()
        {
            return Leavers;
        }

        public CharacterFighter GetLeaver(int characterId)
        {
            return Leavers.OfType<CharacterFighter>().FirstOrDefault(x => x.Id == characterId);
        }

        public ReadOnlyCollection<FightSpectator> GetSpectators()
        {
            return Spectators;
        }

        public IEnumerable<Character> GetCharactersAndSpectators()
        {
            return GetAllCharacters().Concat(GetSpectators().Select(entry => entry.Character));
        }

        public IEnumerable<FightActor> GetFightersAndLeavers()
        {
            return Fighters.Concat(Leavers.Where(x => !(x is CharacterFighter) || !((CharacterFighter)x).IsDisconnected));
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

        public IEnumerable<int> GetDeadFightersIds()
        {
            return GetFightersAndLeavers().Where(entry => entry.IsDead() && entry.IsVisibleInTimeline).Select(entry => entry.Id);
        }

        public IEnumerable<int> GetAliveFightersIds()
        {
            return GetAllFighters<FightActor>(entry => entry.IsAlive() && entry.IsVisibleInTimeline).Select(entry => entry.Id);
        }

        public FightCommonInformations GetFightCommonInformations()
        {
            return new FightCommonInformations(Id,
                                               (sbyte) FightType,
                                               m_teams.Select(entry => entry.GetFightTeamInformations()),
                                               m_teams.Select(entry => entry.BladePosition.Cell.Id),
                                               m_teams.Select(entry => entry.GetFightOptionsInformations()));
        }

        public FightExternalInformations GetFightExternalInformations()
        {
            return new FightExternalInformations(Id, (sbyte)FightType, !IsStarted ? 0 : StartTime.GetUnixTimeStamp(), SpectatorClosed,
                m_teams.Select(entry => entry.GetFightTeamLightInformations()), m_teams.Select(entry => entry.GetFightOptionsInformations()));
        }


        #endregion
    }
}