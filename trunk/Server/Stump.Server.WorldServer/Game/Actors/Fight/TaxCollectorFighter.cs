﻿using System;
using Stump.Server.WorldServer.Game.Actors.RolePlay.TaxCollectors;
using Stump.Server.WorldServer.Game.Actors.Stats;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Maps.Cells;

namespace Stump.Server.WorldServer.Game.Actors.Fight
{
    public class TaxCollectorFighter : AIFighter
    {
        private readonly StatsFields m_stats;

        public TaxCollectorFighter(FightTeam team, TaxCollectorNpc taxCollector)
            : base(team, taxCollector.Spells, taxCollector.GlobalId)
        {
            TaxCollectorNpc = taxCollector;

            m_stats = new StatsFields(this);
        }

        public TaxCollectorNpc TaxCollectorNpc
        {
            get;
            private set;
        }

        public override string Name
        {
            get { return TaxCollectorNpc.Name; }
        }

        public override ObjectPosition MapPosition
        {
            get { return TaxCollectorNpc.Position; }
        }

        public override byte Level
        {
            get { return TaxCollectorNpc.Level; }
        }

        public override StatsFields Stats
        {
            get { throw new NotImplementedException(); }
        }

        public override string GetMapRunningFighterName()
        {
            throw new NotImplementedException();
        }
    }
}
