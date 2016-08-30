﻿using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using System.Linq;

namespace Stump.Server.WorldServer.Game.Fights.Teams
{
    public class FightTaxCollectorAttackersTeam : FightTeamWithLeader<CharacterFighter>
    {
        public FightTaxCollectorAttackersTeam(TeamEnum id, Cell[] placementCells) : base(id, placementCells)
        {
        }

        public FightTaxCollectorAttackersTeam(TeamEnum id, Cell[] placementCells, AlignmentSideEnum alignmentSide)
            : base(id, placementCells, alignmentSide)
        {
        }

        public override TeamTypeEnum TeamType
        {
            get { return TeamTypeEnum.TEAM_TYPE_PLAYER; }
        }

        public override FighterRefusedReasonEnum CanJoin(Character character)
        {
            if (Fight is FightPvT && character.Guild == (Fight as FightPvT).TaxCollector.TaxCollectorNpc.Guild)
                return FighterRefusedReasonEnum.WRONG_GUILD;

            if (Fighters.Where(x => x is CharacterFighter).Any(x => (x as CharacterFighter).Character.Client.IP == character.Client.IP))
                return FighterRefusedReasonEnum.MULTIACCOUNT_NOT_ALLOWED;

            return base.CanJoin(character);
        }
    }
}