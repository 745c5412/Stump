﻿using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Fights.Teams;
using System.Collections.Generic;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;

namespace Stump.Server.WorldServer.Game.Actors.Fight
{
    public abstract class SummonedFighter : AIFighter
    {
        protected SummonedFighter(int id, FightTeam team, IEnumerable<Spell> spells, FightActor summoner, Cell cell)
            : base(team, spells)
        {
            Id = id;

            Position = summoner.Position.Clone();
            FightStartPosition = Position;

            Cell = cell;
            Summoner = summoner;
        }

        protected SummonedFighter(int id, FightTeam team, IEnumerable<Spell> spells, FightActor summoner, Cell cell, int identifier)
            : base(team, spells, identifier)
        {
            Id = id;

            Position = summoner.Position.Clone();
            FightStartPosition = Position;

            Cell = cell;
            Summoner = summoner;
        }

        public override sealed int Id
        {
            get;
            protected set;
        }

        public FightActor Summoner
        {
            get;
            protected set;
        }

        public override bool HasResult => false;

        public override int GetTackledAP() => 0;

        public override int GetTackledMP() => 0;

        protected override void OnDead(FightActor killedBy, bool passTurn = true)
        {
            base.OnDead(killedBy, passTurn);
            Summoner.RemoveSummon(this);
        }
    }
}