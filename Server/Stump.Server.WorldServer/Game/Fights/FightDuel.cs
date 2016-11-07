using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights.Results;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Handlers.Context;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Stump.Server.WorldServer.Game.Fights
{
    public class FightDuel : Fight<FightPlayerTeam, FightPlayerTeam>
    {
        public FightDuel(int id, Map fightMap, FightPlayerTeam defendersTeam, FightPlayerTeam challengersTeam)
            : base(id, fightMap, defendersTeam, challengersTeam)
        {
        }

        public override void StartPlacement()
        {
            base.StartPlacement();

            m_placementTimer = Map.Area.CallDelayed(FightConfiguration.PlacementPhaseTime, StartFighting);
        }

        public override void StartFighting()
        {
            m_placementTimer.Dispose();

            base.StartFighting();
        }

        public override bool IsDeathTemporarily => true;

        public override FightTypeEnum FightType => FightTypeEnum.FIGHT_TYPE_CHALLENGE;

        public override bool IsPvP => true;

        public override bool IsMultiAccountRestricted => false;

        protected override List<IFightResult> GetResults()
        {
            return GetFightersAndLeavers().Where(entry => entry.HasResult).Select(fighter => fighter.GetFightResult()).ToList();
        }

        protected override void SendGameFightJoinMessage(CharacterFighter fighter)
        {
            ContextHandler.SendGameFightJoinMessage(fighter.Character.Client, CanCancelFight(), !IsStarted, false, IsStarted, (int)GetPlacementTimeLeft().TotalMilliseconds, FightType);
        }

        protected override void SendGameFightJoinMessage(FightSpectator spectator)
        {
            ContextHandler.SendGameFightJoinMessage(spectator.Character.Client, false, false, true, IsStarted, (int)GetPlacementTimeLeft().TotalMilliseconds, FightType);
        }

        public TimeSpan GetPlacementTimeLeft()
        {
            var timeleft = TimeSpan.FromMilliseconds(FightConfiguration.PlacementPhaseTime) - (DateTime.Now - CreationTime);

            if (timeleft < TimeSpan.Zero)
                timeleft = TimeSpan.Zero;

            return timeleft;
        }

        protected override bool CanCancelFight()
        {
            return State == FightState.Placement;
        }
    }
}