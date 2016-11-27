using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights.Results;
using Stump.Server.WorldServer.Game.Fights.Results.Data;
using Stump.Server.WorldServer.Game.Fights.Teams;
using System.Collections.Generic;
using FightLoot = Stump.Server.WorldServer.Game.Fights.Results.FightLoot;

namespace Stump.Server.WorldServer.Game.Arena
{
    public class ArenaFightResult : FightResult<CharacterFighter>
    {
        public ArenaFightResult(CharacterFighter fighter, FightOutcomeEnum outcome, FightLoot loot, int rank, bool showLoot = true)
            : base(fighter, outcome, loot)
        {
            Rank = rank;
            ShowLoot = showLoot;
        }

        public override bool CanLoot(FightTeam team) => Outcome == FightOutcomeEnum.RESULT_VICTORY && !Fighter.HasLeft() && ShowLoot;

        public int Rank
        {
            get;
        }

        public bool ShowLoot
        {
            get;
        }

        public override FightResultListEntry GetFightResultListEntry()
        {
            var amount = 0;
            var kamas = 0;
            var xp = 0;

            if (CanLoot(Fighter.Team))
            {
                amount = Fighter.Character.ComputeWonArenaTokens(Rank);
                kamas = Fighter.Character.ComputeWonArenaKamas();
                xp = Fighter.Character.ComputeWonArenaXP();
            }

            var items = amount > 0 ? new[] { (short)ItemIdEnum.Kolizeton, (short)amount } : new short[0];

            var loot = new DofusProtocol.Types.FightLoot(items, kamas);
            var xpData = new FightExperienceData(Fighter.Character)
            {
                ShowExperience = true,
                ShowExperienceFightDelta = true,
                ShowExperienceLevelFloor = true,
                ShowExperienceNextLevelFloor = true
            };
            xpData.ExperienceFightDelta += xp;

            return new FightResultPlayerListEntry((short)Outcome, loot, Id, Alive, (byte)Level, new List<DofusProtocol.Types.FightResultAdditionalData> { xpData.GetFightResultAdditionalData() });
        }

        public override void Apply()
        {
            Fighter.Character.UpdateArenaProperties(Rank, Outcome == FightOutcomeEnum.RESULT_VICTORY);
        }
    }
}