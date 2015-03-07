using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Interfaces;

namespace Stump.Server.WorldServer.Game.Actors.Stats
{
    public class StatsAP : StatsData
    {
        public StatsAP(IStatsOwner owner, int valueBase)
            : base(owner, PlayerFields.AP, valueBase)
        {
        }

        public StatsAP(IStatsOwner owner, int valueBase, int limit)
            : base(owner, PlayerFields.AP, valueBase, limit, true)
        {
        }

        public short Used
        {
            get;
            set;
        }

        public int TotalMax
        {
            get
            {
                return base.Total;
            }
        }

        public override int Total
        {
            get
            {
                return TotalMax - Used;
            }
        }

        public override StatsData Clone()
        {
            var clone = new StatsAP(Owner, ValueBase, Limit ?? 0) 
            {
                Equiped = Equiped,
                Used = 0
            };

            return clone;
        }
    }
}