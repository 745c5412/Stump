using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using Stump.Core.Attributes;
using Stump.Core.Extensions;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Items;

namespace ArkalysPlugin.Daily
{
    public class DailyQuestManager : DataManager<DailyQuestManager>
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        
        [Variable]
        public static int ContractItemId = 20988;

        [Variable]
        public static int TokenItemId = 7919;

        [Variable]
        public static double DailyQuestDifficulty = 10;

        [Variable]
        public static double DailyQuestXpMonsterFactor = 1.5;

        [Variable]
        public static double DailyQuestXpItemFactor = 1000;
        
        private Dictionary<int, DailyObjectiveRecord> m_objectives;

        [Initialization(typeof(ItemManager), Silent=true)]
        public void Initialize()
        {   
            ContractItem = ItemManager.Instance.TryGetTemplate(ContractItemId);

            if (ContractItem == null)
            {
                logger.Error("Item template {0} doesn't exist in database !", ContractItemId);
                return;
            }

            TokenItem = ItemManager.Instance.TryGetTemplate(TokenItemId);

            if (TokenItem == null)
            {
                logger.Error("Item template {0} doesn't exist in database !", TokenItemId);
                return;
            }

            m_objectives = Database.Query<DailyObjectiveRecord>(DailyObjectiveRelator.FetchQuery).ToDictionary(x => x.Id);
        }

        public DailyObjectiveRecord GetObjective(int id)
        {
            DailyObjectiveRecord record;
            return m_objectives.TryGetValue(id, out record) ? record : null;
        }

        public ItemTemplate ContractItem
        {
            get;
            private set;
        }

        public ItemTemplate TokenItem
        {
            get;
            private set;
        }

        public ContractInfo GenerateContract(Character character)
        {
            var rand = new Random(DateTime.Now.DayOfYear);

            var possibleObjectives = m_objectives.Values.Where(x => character.Level >= x.MinLevel && character.Level <= x.MaxLevel).Shuffle(rand).ToArray();

            var objectives = new List<DailyObjectiveRecord>();

            var i = 0;
            var difficulty = 0d;
            while (i < possibleObjectives.Length && difficulty < DailyQuestDifficulty)
            {
                objectives.Add(possibleObjectives[i]);
                difficulty += possibleObjectives[i].Difficulty;
                i++;
            }       

            return new ContractInfo(objectives.ToArray());
        }

        public void GiveContractRewards(Character character, ContractItem contract)
        {
            var monstersXp = contract.Objectives.Where(x => x.Monster != null).Sum(x => x.Monster.Grades.Last().GradeXp * x.Amount * x.Difficulty);
            var itemLevels = contract.Objectives.Where(x => x.Item != null).Sum(x => x.Item.Level*x.Amount*x.Difficulty);

            var xp = DailyQuestXpMonsterFactor*monstersXp + DailyQuestXpItemFactor*itemLevels;
            var tokens = (int)Math.Floor(character.Level*character.Level*contract.Objectives.Sum(x => x.Difficulty) / 1000d);

            character.AddExperience(xp);
            character.Inventory.AddItem(TokenItem, tokens);
        }
    }
}