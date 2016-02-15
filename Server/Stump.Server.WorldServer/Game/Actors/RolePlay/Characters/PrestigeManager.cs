using System.Linq;
using NLog;
using Stump.Core.Attributes;
using Stump.Core.Reflection;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items;

namespace Stump.Server.WorldServer.Game.Actors.RolePlay.Characters
{
    public class PrestigeManager : Singleton<PrestigeManager>
    {
        static readonly Logger logger = LogManager.GetCurrentClassLogger();
        public const int ItemForBonus = 20214;

        public static ItemTemplate BonusItem;

        [Variable] public static short[] PrestigeTitles =
        {
            200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214
        };

        static readonly EffectInteger[][] m_prestigesBonus =
        {
            new[] { new EffectInteger(EffectsEnum.Effect_AddDamageBonus, 2) },
            new[] { new EffectInteger(EffectsEnum.Effect_AddVitality, 25) },
            new[]
            {
                new EffectInteger(EffectsEnum.Effect_AddChance, 20),
                new EffectInteger(EffectsEnum.Effect_AddIntelligence, 20),
                new EffectInteger(EffectsEnum.Effect_AddWisdom, 20),
                new EffectInteger(EffectsEnum.Effect_AddAgility, 20),
                new EffectInteger(EffectsEnum.Effect_AddStrength, 20)
            },
            new[] { new EffectInteger(EffectsEnum.Effect_AddDamageBonus, 3) },
            new[]
            {
                new EffectInteger(EffectsEnum.Effect_AddVitality, 50)
            },
            new[]
            {
                new EffectInteger(EffectsEnum.Effect_AddChance, 30),
                new EffectInteger(EffectsEnum.Effect_AddIntelligence, 30),
                new EffectInteger(EffectsEnum.Effect_AddWisdom, 30),
                new EffectInteger(EffectsEnum.Effect_AddAgility, 30),
                new EffectInteger(EffectsEnum.Effect_AddStrength, 30)
            },
            new[] { new EffectInteger(EffectsEnum.Effect_AddDamageBonus, 5) },
            new[] { new EffectInteger(EffectsEnum.Effect_IncreaseDamage_138, 20) },
            new[] { new EffectInteger(EffectsEnum.Effect_AddDamageBonus, 10) },
            new[]
            {
                new EffectInteger(EffectsEnum.Effect_AddAirElementReduction, 10),
                new EffectInteger(EffectsEnum.Effect_AddEarthElementReduction, 10),
                new EffectInteger(EffectsEnum.Effect_AddFireElementReduction, 10),
                new EffectInteger(EffectsEnum.Effect_AddWaterElementReduction, 10),
                new EffectInteger(EffectsEnum.Effect_AddNeutralElementReduction, 10)
            },
            new[] { new EffectInteger(EffectsEnum.Effect_IncreaseDamage_138, 20) },
            new[] { new EffectInteger(EffectsEnum.Effect_AddVitality, 50) },
            new[] { new EffectInteger(EffectsEnum.Effect_AddCriticalHit, 7) },
            new[] { new EffectInteger(EffectsEnum.Effect_AddRange, 1) },
            new[] {
                new EffectInteger(EffectsEnum.Effect_AddMP_128, 1)
            },

        };

        bool m_disabled;

        public bool PrestigeEnabled => !m_disabled;

        [Initialization(typeof (ItemManager), Silent = true)]
        public void Initialize()
        {
            BonusItem = ItemManager.Instance.TryGetTemplate(ItemForBonus);

            if (BonusItem != null)
                return;

            logger.Error("Item {0} not found, prestiges disabled", ItemForBonus);
            m_disabled = true;
        }

        public static EffectInteger[] GetPrestigeEffects(int rank) => m_prestigesBonus.Take(rank).SelectMany(x => x.Select(y => (EffectInteger)y.Clone())).ToArray();

        public static short GetPrestigeTitle(int rank) => PrestigeTitles[rank - 1];
    }
}