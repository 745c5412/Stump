using NLog;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items;
using System.Linq;

namespace ScriptsPlugin
{
    public class RegenItem
    {
        public RegenItem(int itemId, bool max)
        {
            ItemId = itemId;
            Max = max;
        }

        public int ItemId
        {
            get;
        }

        public bool Max
        {
            get;
        }
    }

    public static class RegenItems
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static RegenItem[] Items =
        {
            new RegenItem(13483, false),
            new RegenItem(13484, false),
            new RegenItem(13485, false),
            new RegenItem(13486, false),
            new RegenItem(20399, false),
            new RegenItem(20349, false),
            new RegenItem(20350, false),
            new RegenItem(20351, false),
            new RegenItem(20352, false),
            new RegenItem(20353, false),
            new RegenItem(20354, false),
            new RegenItem(20355, false),
            new RegenItem(20356, false),
            new RegenItem(20357, false),
            new RegenItem(20358, false),
            new RegenItem(20359, false),
            new RegenItem(20360, false),
            new RegenItem(20361, false),
            new RegenItem(20362, false),
        };

        [Initialization(InitializationPass.Last)]
        public static void Initialize()
        {
            var allItems = World.Instance.Database.Fetch<PlayerItemRecord>(string.Format(PlayerItemRelator.FetchQuery));

            foreach (var item in allItems.Where(x => Items.Any(y => y.ItemId == x.ItemId)))
            {
                var itemMax = Items.FirstOrDefault(x => x.ItemId == item.ItemId).Max;

                if (item.Effects.Any(x => x.EffectId == EffectsEnum.Effect_LivingObjectId))
                {
                    var m_livingObjectIdEffect = (EffectInteger)item.Effects.FirstOrDefault(x => x.EffectId == EffectsEnum.Effect_LivingObjectId);
                    var m_moodEffect = (EffectInteger)item.Effects.FirstOrDefault(x => x.EffectId == EffectsEnum.Effect_LivingObjectMood);
                    var m_selectedLevelEffect = (EffectInteger)item.Effects.FirstOrDefault(x => x.EffectId == EffectsEnum.Effect_LivingObjectSkin);
                    var m_experienceEffect = (EffectInteger)item.Effects.FirstOrDefault(x => x.EffectId == EffectsEnum.Effect_LivingObjectLevel);
                    var m_categoryEffect = (EffectInteger)item.Effects.FirstOrDefault(x => x.EffectId == EffectsEnum.Effect_LivingObjectCategory);

                    item.Effects = ItemManager.Instance.GenerateItemEffects(item.Template, itemMax);

                    item.Effects.Add(m_livingObjectIdEffect);
                    item.Effects.Add(m_moodEffect);
                    item.Effects.Add(m_selectedLevelEffect);
                    item.Effects.Add(m_experienceEffect);
                    item.Effects.Add(m_categoryEffect);
                }
                else
                {
                    item.Effects = ItemManager.Instance.GenerateItemEffects(item.Template, itemMax);
                }

                World.Instance.Database.Update(item);

                Logger.Info("Update Item {0}", item.ItemId);
            }
        }
    }
}