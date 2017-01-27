using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Fights.Challenges;
using Stump.Server.WorldServer.Game.Fights.Sequences;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Game.Fights.Triggers;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Game.Maps.Cells;

namespace Stump.Server.WorldServer.AI.Fights
{
    public class AIFightCopy : FightBase, IFight
    {
        public IFight Original
        {
            get;
        }

        private List<FightActor> m_currentCopiedFighter = new List<FightActor>();
        private Dictionary<FightActor, FightActor> m_fightersCopies = new Dictionary<FightActor, FightActor>();

        public AIFightCopy(IFight original)
            :base(original.Id, original.Map, new AITeamCopy(original.DefendersTeam), new AITeamCopy(original.ChallengersTeam))
        {
            Original = original;
            DefendersTeam.Fight = this;
            ChallengersTeam.Fight = this;
            

            DefendersTeam.FighterAdded += OnFighterAdded;
            DefendersTeam.FighterRemoved += OnFighterRemoved;
            ChallengersTeam.FighterAdded += OnFighterAdded;
            ChallengersTeam.FighterRemoved += OnFighterRemoved;

            CreationTime = original.CreationTime;
        }

        private FightActor CreateFighterCopy(FightActor original)
        {
            if (m_currentCopiedFighter.Contains(original))
                throw new Exception("Reference cycle detected");

            m_currentCopiedFighter.Add(original);

            var copy = original.GetAICopy(this);
            m_fightersCopies.Add(original, copy);

            m_currentCopiedFighter.Remove(original);

            return copy;
        }

        public FightActor GetFighterCopy(FightActor original)
        {
            FightActor copy;
            return m_fightersCopies.TryGetValue(original, out copy) ? copy : CreateFighterCopy(original);
        }

        public int GetChallengeBonus()
        {
            return 0;
        }

        IEnumerable<Character> IFight.GetAllCharacters()
        {
            throw new NotImplementedException();
        }

        void IFight.ForEach(Action<Character> action)
        {
            throw new NotImplementedException();
        }

        public void ForEach(Action<Character> action, bool withSpectators = false)
        {
            throw new NotImplementedException();
        }

        public void ForEach(Action<Character> action, Character except, bool withSpectators = false)
        {
            throw new NotImplementedException();
        }


        public TimeSpan GetFightDuration()
        {
            throw new NotImplementedException();
        }

        public TimeSpan GetTurnTimeLeft()
        {
            throw new NotImplementedException();
        }

        public TimeSpan GetPlacementTimeLeft()
        {
            throw new NotImplementedException();
        }
        
        public ReadOnlyCollection<FightActor> GetLeavers()
        {
            throw new NotImplementedException();
        }

        public CharacterFighter GetLeaver(int characterId)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<FightSpectator> GetSpectators()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Character> GetCharactersAndSpectators()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FightActor> GetFightersAndLeavers()
        {
            throw new NotImplementedException();
        }
        
        public IEnumerable<int> GetDeadFightersIds()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<int> GetAliveFightersIds()
        {
            throw new NotImplementedException();
        }

        public FightExternalInformations GetFightExternalInformations(Character character)
        {
            throw new NotImplementedException();
        }
        
        public void RejoinFightFromDisconnection(CharacterFighter character)
        {
            throw new NotImplementedException();
        }

        public void RefreshActor(FightActor actor)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Character> GetAllCharacters(bool withSpectators = false)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Character> ICharacterContainer.GetAllCharacters()
        {
            throw new NotImplementedException();
        }

        void ICharacterContainer.ForEach(Action<Character> action)
        {
            throw new NotImplementedException();
        }

        WorldClientCollection IFight.Clients
        {
            get { throw new NotImplementedException(); }
        }
        

        public bool AIDebugMode
        {
            get;
            set;
        }

        public bool Freezed
        {
            get;
            set;
        }

        public event Action<IFight> FightStarted;
        public event Action<IFight> FightEnded;
        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void StartFighting()
        {
            throw new NotImplementedException();
        }
        

        public void CancelFight()
        {
            throw new NotImplementedException();
        }

        public override void EndFight()
        {
            throw new NotImplementedException();
        }

        public event FightWinnersDelegate WinnersDetermined;
        public event ResultsGeneratedDelegate ResultsGenerated;
        public event Action<IFight> GeneratingResults;
        public void StartPlacement()
        {
            throw new NotImplementedException();
        }

        public void ShowBlades()
        {
            throw new NotImplementedException();
        }

        public void HideBlades()
        {
            throw new NotImplementedException();
        }

        public void UpdateBlades(FightTeam team)
        {
            throw new NotImplementedException();
        }

        public bool FindRandomFreeCell(FightActor fighter, out Cell cell, bool placement = true)
        {
            throw new NotImplementedException();
        }

        public bool RandomnizePosition(FightActor fighter)
        {
            throw new NotImplementedException();
        }

        public void RandomnizePositions(FightTeam team)
        {
            throw new NotImplementedException();
        }

        public DirectionsEnum FindPlacementDirection(FightActor fighter)
        {
            throw new NotImplementedException();
        }

        public bool KickFighter(FightActor kicker, FightActor fighter)
        {
            throw new NotImplementedException();
        }

        public bool CanChangePosition(FightActor fighter, Cell cell)
        {
            throw new NotImplementedException();
        }

        public void ToggleSpectatorClosed(Character character, bool state)
        {
            throw new NotImplementedException();
        }

        public bool CanSpectatorJoin(Character spectator)
        {
            throw new NotImplementedException();
        }

        public bool AddSpectator(FightSpectator spectator)
        {
            throw new NotImplementedException();
        }

        public void RemoveSpectator(FightSpectator spectator)
        {
            throw new NotImplementedException();
        }

        public void RemoveAllSpectators()
        {
            throw new NotImplementedException();
        }
        

        public event Action<IFight, FightActor> TurnStarted;

        public event Action<IFight, FightActor> BeforeTurnStopped;
        public event Action<IFight, FightActor> TurnStopped;
        public IEnumerable<Buff> GetBuffs()
        {
            throw new NotImplementedException();
        }

        public void UpdateBuff(Buff buff, bool updateAction = true)
        {
            throw new NotImplementedException();
        }
        
        public IEnumerable<MarkTrigger> GetTriggers()
        {
            throw new NotImplementedException();
        }

        public void SetChallenge(DefaultChallenge challenge)
        {
            throw new NotImplementedException();
        }

        public override FightTypeEnum FightType => Original.FightType;

        public DateTime CreationTime
        {
            get;
        }
        
        WorldClientCollection ICharacterContainer.Clients
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsPvP
        {
            get;
        }

        public bool IsMultiAccountRestricted
        {
            get;
        }

        public bool IsStarted
        {
            get;
        }

        public DateTime StartTime
        {
            get;
        }

        public short AgeBonus
        {
            get;
        }

        public ReadOnlyCollection<FightActor> Leavers
        {
            get;
        }

        public ReadOnlyCollection<FightSpectator> Spectators
        {
            get;
        }

        public DefaultChallenge Challenge
        {
            get;
        }

        public DateTime TurnStartTime
        {
            get;
        }

        public ReadyChecker ReadyChecker
        {
            get;
        }

        public bool SpectatorClosed
        {
            get;
        }

        public bool BladesVisible
        {
            get;
        }

        public bool IsDeathTemporarily
        {
            get;
        }

        public bool CanKickPlayer
        {
            get;
        }

        public WorldClientCollection SpectatorClients
        {
            get;
        }
    }
}