using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Fights.Results.Data;
using Stump.Server.WorldServer.Game.Guilds;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Game.Items.Player;
using Stump.Server.WorldServer.Handlers.Characters;
using FightLoot = Stump.Server.WorldServer.Game.Fights.Loots.FightLoot;
using FightResultAdditionalData = Stump.Server.WorldServer.Game.Fights.Results.Data.FightResultAdditionalData;

namespace Stump.Server.WorldServer.Game.Fights.Results
{
    public class FightPlayerResult : FightResult<CharacterFighter>, IExperienceResult, IPvpResult
    {
        public FightPlayerResult(CharacterFighter fighter, FightOutcomeEnum outcome, FightLoot loot)
            : base(fighter, outcome, loot)
        {
        }

        public Character Character
        {
            get { return Fighter.Character; }
        }

        public byte Level
        {
            get { return Character.Level; }
        }

        public override bool CanLoot(FightTeam team)
        {
            return Fighter.Team == team;
        }

        public FightExperienceData ExperienceData
        {
            get;
            private set;
        }

        public FightPvpData PvpData
        {
            get;
            private set;
        }

        public override FightResultListEntry GetFightResultListEntry()
        {
            var additionalDatas =
                new List<DofusProtocol.Types.FightResultAdditionalData>();

            if (ExperienceData != null)
                additionalDatas.Add(ExperienceData.GetFightResultAdditionalData());

            if (PvpData != null)
                additionalDatas.Add(PvpData.GetFightResultAdditionalData());

            return new FightResultPlayerListEntry((short) Outcome, Loot.GetFightLoot(), Id, Alive, Level,
                additionalDatas);
        }

        public override void Apply()
        {
            Character.Inventory.AddKamas(Loot.Kamas);

            foreach (DroppedItem drop in Loot.Items.Values)
            {
                ItemTemplate template = ItemManager.Instance.TryGetTemplate(drop.ItemId);

                if (template.Effects.Count > 0)
                    for (int i = 0; i < drop.Amount; i++)
                    {
                        BasePlayerItem item = ItemManager.Instance.CreatePlayerItem(Character, drop.ItemId, 1);
                        Character.Inventory.AddItem(item);
                    }
                else
                {
                    BasePlayerItem item = ItemManager.Instance.CreatePlayerItem(Character, drop.ItemId, drop.Amount);
                    Character.Inventory.AddItem(item);
                }
            }
            if (ExperienceData != null)
                ExperienceData.Apply();

            if (PvpData != null)
                PvpData.Apply();

            CharacterHandler.SendCharacterStatsListMessage(Character.Client);
        }

        public void SetEarnedExperience(int experience)
        {
            if (Fighter.HasLeft())
                return;

            if (ExperienceData == null)
                ExperienceData = new FightExperienceData(Character);

            var guildXp = 0;
            if (Character.GuildMember != null && Character.GuildMember.GivenPercent > 0)
            {
                var xp = (int)(experience*(Character.GuildMember.GivenPercent*0.01));
                guildXp = (int)Character.Guild.AdjustGivenExperience(Character, xp);

                guildXp = guildXp > Guild.MaxGuildXP ? Guild.MaxGuildXP : guildXp;
                experience -= guildXp;

                if (guildXp > 0)
                {
                    ExperienceData.ShowExperienceForGuild = true;
                    ExperienceData.ExperienceForGuild = guildXp;
                }
            }

            ExperienceData.ShowExperienceFightDelta = true;
            ExperienceData.ShowExperience = true;
            ExperienceData.ShowExperienceLevelFloor = true;
            ExperienceData.ShowExperienceNextLevelFloor = true;
            ExperienceData.ExperienceFightDelta = experience;
        }

        public void SetEarnedHonor(short honor, short dishonor)
        {
            if (PvpData == null)
                PvpData = new FightPvpData(Character);

            PvpData.HonorDelta = honor;
            PvpData.DishonorDelta = dishonor;
            PvpData.Honor = Character.Honor;
            PvpData.Dishonor = Character.Dishonor;
            PvpData.Grade = (byte) Character.AlignmentGrade;
            PvpData.MinHonorForGrade = Character.LowerBoundHonor;
            PvpData.MaxHonorForGrade = Character.UpperBoundHonor;
        }
    }
}