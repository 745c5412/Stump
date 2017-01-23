using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NLog;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Spells;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Effects.Handlers;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.AI.Fights.Spells
{
    public static class SpellIdentifier
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private static readonly Dictionary<EffectsEnum, SpellCategory> m_categories = new Dictionary<EffectsEnum, SpellCategory>();

        [Initialization(typeof(EffectManager))]
        public static void Initialize()
        {
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(entry => entry.IsSubclassOf(typeof(SpellEffectHandler)) && !entry.IsAbstract))
            {
                if (type.GetCustomAttribute<DefaultEffectHandlerAttribute>() != null)
                    continue; // we don't mind about default handlers

                var attributes = type.GetCustomAttributes<EffectHandlerAttribute>().ToArray();

                if (attributes.Length == 0)
                {
                    logger.Error("EffectHandler '{0}' has no EffectHandlerAttribute", type.Name);
                    continue;
                }

                var aiAttributes = type.GetCustomAttributes<AISpellEffectAttribute>().ToArray();
                foreach (var attribute in aiAttributes)
                    if (attribute.Effect == null)
                        foreach (var effect in attributes.Select(entry => entry.Effect))
                            SetEffectCategory(effect, attribute.Category);
                    else
                        SetEffectCategory(attribute.Effect.Value, attribute.Category);
            }
        }

        public static void SetEffectCategory(EffectsEnum effect, SpellCategory category)
        {
            if (m_categories.ContainsKey(effect))
                m_categories[effect] |= category;
            else
                m_categories.Add(effect, category);
        }

        public static SpellCategory GetSpellCategories(Spell spell) => GetSpellCategories(spell.CurrentSpellLevel);

        public static SpellCategory GetSpellCategories(SpellLevelTemplate spellLevel)
            => spellLevel.Effects.Aggregate(SpellCategory.None, (current, effect) => current | GetEffectCategories(effect.EffectId));
        
        public static SpellCategory GetEffectCategories(EffectsEnum effectId)
        {
            SpellCategory category;
            return m_categories.TryGetValue(effectId, out category) ? category : SpellCategory.None;
        }
    }
}