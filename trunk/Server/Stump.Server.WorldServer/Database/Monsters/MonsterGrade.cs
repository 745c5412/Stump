using System.Collections.Generic;
using System.Linq;
using ServiceStack.Text;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.Enums;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters;

namespace Stump.Server.WorldServer.Database.Monsters
{
    public class MonsterGradeRelator
    {
        public static string FetchQuery = "SELECT * FROM monsters_grades";
    }

    [TableName("monsters_grades")]
    [D2OClass("MonsterGrade", "com.ankamagames.dofus.datacenter.monsters")]
    public class MonsterGrade : IAssignedByD2O, ISaveIntercepter, IAutoGeneratedRecord
    {
        private List<MonsterSpell> m_spellsTemplates;
        private string m_statsJSON;
        private MonsterTemplate m_template;
        private int m_wisdom;
        private List<Game.Spells.Spell> m_spells;

        public MonsterGrade()
        {
            Stats = new Dictionary<PlayerFields, short>();
        }

        public int Id
        {
            get;
            set;
        }

        public uint GradeId
        {
            get;
            set;
        }

        public int GradeXp
        {
            get;
            set;
        }

        public int MonsterId
        {
            get;
            set;
        }

        [Ignore]
        public MonsterTemplate Template
        {
            get { return m_template ?? (m_template = MonsterManager.Instance.GetTemplate(MonsterId)); }
            set
            {
                m_template = value;
                MonsterId = value.Id;
            }
        }

        public uint Level
        {
            get;
            set;
        }

        public int PaDodge
        {
            get;
            set;
        }

        public int PmDodge
        {
            get;
            set;
        }

        public int EarthResistance
        {
            get;
            set;
        }

        public int AirResistance
        {
            get;
            set;
        }

        public int FireResistance
        {
            get;
            set;
        }

        public int WaterResistance
        {
            get;
            set;
        }

        public int NeutralResistance
        {
            get;
            set;
        }

        public int LifePoints
        {
            get;
            set;
        }

        public int ActionPoints
        {
            get;
            set;
        }

        public int MovementPoints
        {
            get;
            set;
        }

        public short TackleEvade
        {
            get;
            set;
        }

        public short TackleBlock
        {
            get;
            set;
        }

        public short Strength
        {
            get;
            set;
        }

        public short Chance
        {
            get;
            set;
        }

        public short Vitality
        {
            get;
            set;
        }

        public short Wisdom
        {
            get { return (short) m_wisdom; }
            set { m_wisdom = value; }
        }

        public short Intelligence
        {
            get;
            set;
        }

        public short Agility
        {
            get;
            set;
        }

        public string StatsJSON
        {
            get { return m_statsJSON; }
            set
            {
                m_statsJSON = value;

                if (value == null)
                    Stats = new Dictionary<PlayerFields, short>();
                else
                    Stats = value.FromJson<Dictionary<PlayerFields, short>>();
            }
        }

        [Ignore]
        public Dictionary<PlayerFields, short> Stats
        {
            get;
            set;
        }

        public List<MonsterSpell> SpellsTemplates
        {
            get { return m_spellsTemplates ?? (m_spellsTemplates = MonsterManager.Instance.GetMonsterGradeSpells(Id)); }
        }

        public List<Game.Spells.Spell> Spells
        {
            get
            {
                return m_spells ?? ( m_spells = SpellsTemplates.Select(entry => new Game.Spells.Spell(entry)).ToList());
            }
        }

        public void ReloadSpells()
        {
            m_spells = null;
        }

        #region IAssignedByD2O Members

        public void AssignFields(object d2oObject)
        {
            var grade = (DofusProtocol.D2oClasses.MonsterGrade) d2oObject;
            GradeId = grade.grade;
            GradeXp = grade.gradeXp;
            MonsterId = grade.monsterId;
            Level = grade.level;
            PaDodge = grade.paDodge;
            PmDodge = grade.pmDodge;
            EarthResistance = grade.earthResistance;
            AirResistance = grade.airResistance;
            FireResistance = grade.fireResistance;
            WaterResistance = grade.waterResistance;
            NeutralResistance = grade.neutralResistance;
            LifePoints = grade.lifePoints;
            ActionPoints = grade.actionPoints;
            MovementPoints = grade.movementPoints;
            Wisdom = (short) grade.wisdom;
        }

        #endregion

        #region ISaveIntercepter Members

        public void BeforeSave(bool insert)
        {
            m_statsJSON = Stats.ToJson();
        }

        #endregion
    }
}