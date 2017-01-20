using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Monsters;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Stats;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Game.Maps.Cells;
using System.Globalization;

namespace Stump.Server.WorldServer.Game.Actors.Fight
{
    public class SummonedMonster : SummonedFighter
    {
        private readonly StatsFields m_stats;

        public SummonedMonster(int id, FightTeam team, FightActor summoner, MonsterGrade template, Cell cell)
            : base(id, team, template.Spells.ToArray(), summoner, cell, template.MonsterId)
        {
            Monster = template;
            Look = Monster.Template.EntityLook.Clone();
            m_stats = new StatsFields(this);
            m_stats.Initialize(template);

            if (Monster.Template.Race.SuperRaceId == 28) //Invocations
                AdjustStats();

            Frozen = !template.Template.CanPlay;
        }

        private void AdjustStats()
        {
            // +1% bonus per level
            m_stats.Health.Base = (short)(m_stats.Health.Base * (1 + (Summoner.Level / 100d)));
            m_stats.Intelligence.Base = (short)(m_stats.Intelligence.Base * (1 + (Summoner.Level / 100d)));
            m_stats.Chance.Base = (short)(m_stats.Chance.Base * (1 + (Summoner.Level / 100d)));
            m_stats.Strength.Base = (short)(m_stats.Strength.Base * (1 + (Summoner.Level / 100d)));
            m_stats.Agility.Base = (short)(m_stats.Agility.Base * (1 + (Summoner.Level / 100d)));
            m_stats.Wisdom.Base = (short)(m_stats.Wisdom.Base * (1 + (Summoner.Level / 100d)));
        }

        public override int CalculateArmorValue(int reduction) => (int)(reduction * (100 + 5 * Summoner.Level) / 100d);

        public MonsterGrade Monster
        {
            get;
        }

        public override ObjectPosition MapPosition => Position;

        public override short Level => (short)Monster.Level;

        public override StatsFields Stats => m_stats;

        public override string GetMapRunningFighterName() => Monster.Id.ToString(CultureInfo.InvariantCulture);

        public override string Name => Monster.Template.Name;

        public override FightTeamMemberInformations GetFightTeamMemberInformations() => new FightTeamMemberMonsterInformations(Id, Monster.Template.Id, (sbyte)Monster.GradeId);

        public override GameFightFighterInformations GetGameFightFighterInformations(WorldClient client = null)
        {
            return new GameFightMonsterInformations(
                Id,
                Look.GetEntityLook(),
                GetEntityDispositionInformations(client),
                (sbyte)Team.Id,
                IsAlive(),
                GetGameFightMinimalStats(client),
                (short)Monster.Template.Id,
                (sbyte)Monster.GradeId);
        }

        public override GameFightMinimalStats GetGameFightMinimalStats(WorldClient client = null)
        {
            return new GameFightMinimalStats(
                Stats.Health.Total,
                Stats.Health.TotalMax,
                Stats.Health.Base,
                Stats[PlayerFields.PermanentDamagePercent].Total,
                Stats.Shield.TotalSafe,
                (short)Stats.AP.Total,
                (short)Stats.AP.TotalMax,
                (short)Stats.MP.Total,
                (short)Stats.MP.TotalMax,
                Summoner.Id,
                true,
                (short)Stats[PlayerFields.NeutralResistPercent].Total,
                (short)Stats[PlayerFields.EarthResistPercent].Total,
                (short)Stats[PlayerFields.WaterResistPercent].Total,
                (short)Stats[PlayerFields.AirResistPercent].Total,
                (short)Stats[PlayerFields.FireResistPercent].Total,
                (short)Stats[PlayerFields.NeutralElementReduction].Total,
                (short)Stats[PlayerFields.EarthElementReduction].Total,
                (short)Stats[PlayerFields.WaterElementReduction].Total,
                (short)Stats[PlayerFields.AirElementReduction].Total,
                (short)Stats[PlayerFields.FireElementReduction].Total,
                (short)Stats[PlayerFields.PushDamageReduction].Total,
                (short)Stats[PlayerFields.CriticalDamageReduction].Total,
                (short)Stats[PlayerFields.DodgeAPProbability].Total,
                (short)Stats[PlayerFields.DodgeMPProbability].Total,
                (short)Stats[PlayerFields.TackleBlock].Total,
                (short)Stats[PlayerFields.TackleEvade].Total,
                (sbyte)(client == null ? VisibleState : GetVisibleStateFor(client.Character)) // invisibility state
                );
        }
    }
}