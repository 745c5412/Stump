using Stump.Server.WorldServer.Worlds.Actors.Fight;
using Stump.Server.WorldServer.Worlds.Fights;
using Stump.Server.WorldServer.Worlds.Maps;
using Stump.Server.WorldServer.Worlds.Maps.Pathfinding;

namespace Stump.Server.WorldServer.AI.Fights.Actions
{
    public class AIFightCellsInformationProvider : FigthCellsInformationProvider
    {
        public AIFightCellsInformationProvider(Fight fight, AIFighter fighter)
            : base(fight)
        {
            Fighter = fighter;
        }

        public AIFighter Fighter
        {
            get;
            private set;
        }

        public override CellInformation GetCellInformation(short cell)
        {
            return new CellInformation(Fight.Map.Cells[cell], IsCellWalkable(cell), true, true, 1, null, null);
        }
    }
}