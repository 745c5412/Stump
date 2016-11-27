using MongoDB.Bson;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.BaseServer.Logging;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights.Teams;
using System;
using System.Globalization;

namespace Stump.Server.WorldServer.Game.Fights.Results
{
    public class FightResult<T> : IFightResult where T : FightActor
    {
        public FightResult(T fighter, FightOutcomeEnum outcome, FightLoot loot)
        {
            Fighter = fighter;
            Outcome = outcome;
            Loot = loot;
        }

        public T Fighter
        {
            get;
            protected set;
        }

        #region IFightResult Members

        public bool Alive => Fighter.IsAlive();

        public bool HasLeft => Fighter.HasLeft();

        public int Id => Fighter.Id;

        public int Prospecting => Fighter.Stats[PlayerFields.Prospecting].Total;

        public int Wisdom => Fighter.Stats[PlayerFields.Wisdom].Total;

        public int Level => Fighter.Level;

        public virtual bool CanLoot(FightTeam team) => false;

        public IFight Fight => Fighter.Fight;

        public FightLoot Loot
        {
            get;
            protected set;
        }

        public FightOutcomeEnum Outcome
        {
            get;
            protected set;
        }

        public virtual FightResultListEntry GetFightResultListEntry() => new FightResultFighterListEntry((short)Outcome, Loot.GetFightLoot(), Id, Alive);

        public virtual void Apply()
        {
        }

        #endregion IFightResult Members
    }

    public class FightResult : FightResult<FightActor>
    {
        public FightResult(FightActor fighter, FightOutcomeEnum outcome, FightLoot loot)
            : base(fighter, outcome, loot)
        {
        }
    }

    public interface IFightResult
    {
        bool Alive
        {
            get;
        }

        bool HasLeft
        {
            get;
        }

        int Id
        {
            get;
        }

        int Prospecting
        {
            get;
        }

        int Wisdom
        {
            get;
        }

        int Level
        {
            get;
        }

        FightLoot Loot
        {
            get;
        }

        FightOutcomeEnum Outcome
        {
            get;
        }

        IFight Fight
        {
            get;
        }

        bool CanLoot(FightTeam looters);

        FightResultListEntry GetFightResultListEntry();

        void Apply();
    }
}