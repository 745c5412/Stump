using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Fights.Teams;

namespace Stump.Server.WorldServer.AI.Fights
{
    public class AITeamCopy : FightTeam
    {
        private readonly FightTeam m_original;

        public AITeamCopy(FightTeam original)
            : base(original.Id, original.PlacementCells, original.AlignmentSide)
        {
            m_original = original;
        }

        public override TeamTypeEnum TeamType => m_original.TeamType;
    }
}