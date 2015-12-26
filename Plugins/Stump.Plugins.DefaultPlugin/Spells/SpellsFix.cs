﻿using System;
using System.Linq;
using NLog;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Spells;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Plugins.DefaultPlugin.Spells
{
    public static class SpellsFix
    {
        static readonly Logger logger = LogManager.GetCurrentClassLogger();

        [Initialization(typeof(SpellManager), Silent = true)]
        public static void ApplyFix()
        {
            logger.Debug("Apply spells fix");

            #region XELOR

            RemoveEffectOnAllLevels((int)SpellIdEnum.TÉLÉFRAG_5486, 1, false);
            RemoveEffectOnAllLevels((int)SpellIdEnum.TÉLÉFRAG_5486, 1, false);

            RemoveEffectOnAllLevels((int)SpellIdEnum.TÉLÉFRAG_5487, 1, false);
            RemoveEffectOnAllLevels((int)SpellIdEnum.TÉLÉFRAG_5487, 1, false);

            RemoveEffectOnAllLevels((int)SpellIdEnum.TÉLÉFRAG_5488, 1, false);
            RemoveEffectOnAllLevels((int)SpellIdEnum.TÉLÉFRAG_5488, 1, false);

            RemoveEffectOnAllLevels((int)SpellIdEnum.TÉLÉFRAG_5489, 1, false);
            RemoveEffectOnAllLevels((int)SpellIdEnum.TÉLÉFRAG_5489, 1, false);

            RemoveEffectOnAllLevels((int)SpellIdEnum.TÉLÉFRAG_5490, 1, false);
            RemoveEffectOnAllLevels((int)SpellIdEnum.TÉLÉFRAG_5490, 1, false);

            RemoveEffectOnAllLevels((int)SpellIdEnum.TÉLÉFRAG_5491, 1, false);
            RemoveEffectOnAllLevels((int)SpellIdEnum.TÉLÉFRAG_5491, 1, false);

            RemoveEffectOnAllLevels((int)SpellIdEnum.TÉLÉFRAG_5492, 1, false);
            RemoveEffectOnAllLevels((int)SpellIdEnum.TÉLÉFRAG_5492, 1, false);

            #endregion XELOR

            #region ECAFLIP

            FixEffectOnAllLevels((int)SpellIdEnum.REKOP, 5, (level, effect, critical) => effect.Delay = 0);
            FixEffectOnAllLevels((int)SpellIdEnum.REKOP, 5, (level, effect, critical) => effect.Random = 0);
            FixEffectOnAllLevels((int)SpellIdEnum.REKOP, 6, (level, effect, critical) => effect.Delay = 0);
            FixEffectOnAllLevels((int)SpellIdEnum.REKOP, 6, (level, effect, critical) => effect.Random = 0);
            FixEffectOnAllLevels((int)SpellIdEnum.REKOP, 7, (level, effect, critical) => effect.Delay = 0);
            FixEffectOnAllLevels((int)SpellIdEnum.REKOP, 7, (level, effect, critical) => effect.Random = 0);
            FixEffectOnAllLevels((int)SpellIdEnum.REKOP, 8, (level, effect, critical) => effect.Delay = 0);
            FixEffectOnAllLevels((int)SpellIdEnum.REKOP, 8, (level, effect, critical) => effect.Random = 0);
            FixEffectOnAllLevels((int)SpellIdEnum.REKOP, 9, (level, effect, critical) => effect.Delay = 0);
            FixEffectOnAllLevels((int)SpellIdEnum.REKOP, 9, (level, effect, critical) => effect.Random = 0);
            FixEffectOnAllLevels((int)SpellIdEnum.REKOP, 10, (level, effect, critical) => effect.Delay = 0);
            FixEffectOnAllLevels((int)SpellIdEnum.REKOP, 10, (level, effect, critical) => effect.Random = 0);
            FixEffectOnAllLevels((int)SpellIdEnum.REKOP, 11, (level, effect, critical) => effect.Delay = 0);
            FixEffectOnAllLevels((int)SpellIdEnum.REKOP, 11, (level, effect, critical) => effect.Random = 0);
            FixEffectOnAllLevels((int)SpellIdEnum.REKOP, 12, (level, effect, critical) => effect.Delay = 0);
            FixEffectOnAllLevels((int)SpellIdEnum.REKOP, 12, (level, effect, critical) => effect.Random = 0);
            FixEffectOnAllLevels((int)SpellIdEnum.REKOP, 13, (level, effect, critical) => effect.Delay = 0);
            FixEffectOnAllLevels((int)SpellIdEnum.REKOP, 13, (level, effect, critical) => effect.Random = 0);
            FixEffectOnAllLevels((int)SpellIdEnum.REKOP, 14, (level, effect, critical) => effect.Delay = 0);
            FixEffectOnAllLevels((int)SpellIdEnum.REKOP, 14, (level, effect, critical) => effect.Random = 0);
            FixEffectOnAllLevels((int)SpellIdEnum.REKOP, 15, (level, effect, critical) => effect.Delay = 0);
            FixEffectOnAllLevels((int)SpellIdEnum.REKOP, 15, (level, effect, critical) => effect.Random = 0);
            FixEffectOnAllLevels((int)SpellIdEnum.REKOP, 16, (level, effect, critical) => effect.Delay = 0);
            FixEffectOnAllLevels((int)SpellIdEnum.REKOP, 16, (level, effect, critical) => effect.Random = 0);

            #endregion ECAFLIP

            #region STEAMER

            FixEffectOnAllLevels((int) SpellIdEnum.HARPONNEUSE, 1, (level, effect, critical) => effect.Priority = 1);

            FixEffectOnAllLevels((int)SpellIdEnum.GARDIENNE, 1, (level, effect, critical) => effect.Priority = 1);

            FixEffectOnAllLevels((int)SpellIdEnum.TACTIRELLE, 1, (level, effect, critical) => effect.Priority = 1);


            #endregion STEAMER
        }

        #region Methods

        public static void FixEffectOnAllLevels(int spellId, int effectIndex, Action<SpellLevelTemplate, EffectDice, bool> fixer, bool critical = true)
        {
            var spellLevels = SpellManager.Instance.GetSpellLevels(spellId).ToArray();

            if (spellLevels.Length == 0)
                throw new Exception($"Cannot apply fix on spell {spellId} : spell do not exists");

            foreach (var level in spellLevels)
            {
                fixer(level, level.Effects[effectIndex], false);
                if (critical && level.CriticalEffects.Count > effectIndex)
                    fixer(level, level.CriticalEffects[effectIndex], true);
            }
        }

        public static void FixEffectOnAllLevels(int spellId, EffectsEnum effect, Action<SpellLevelTemplate, EffectDice, bool> fixer, bool critical = true)
        {
            var spellLevels = SpellManager.Instance.GetSpellLevels(spellId).ToArray();

            if (spellLevels.Length == 0)
            {
                logger.Error($"Cannot apply fix on spell {spellId} : spell do not exists");
                return;
            }

            foreach (var level in spellLevels)
            {
                foreach (var spellEffect in level.Effects.Where(entry => entry.EffectId == effect))
                {
                    fixer(level, spellEffect, false);
                }

                if (!critical)
                    continue;

                foreach (var spellEffect in level.CriticalEffects.Where(entry => entry.EffectId == effect))
                {
                    fixer(level, spellEffect, true);
                }
            }
        }

        public static void FixCriticalEffectOnAllLevels(int spellId, int effectIndex, Action<SpellLevelTemplate, EffectDice, bool> fixer)
        {
            var spellLevels = SpellManager.Instance.GetSpellLevels(spellId).ToArray();

            if (spellLevels.Length == 0)
                throw new Exception($"Cannot apply fix on spell {spellId} : spell do not exists");

            foreach (var level in spellLevels)
            {
                fixer(level, level.CriticalEffects[effectIndex], true);
            }
        }

        public static void FixEffectOnAllLevels(int spellId, Predicate<EffectDice> predicate, Action<SpellLevelTemplate, EffectDice, bool> fixer, bool critical = true)
        {
            var spellLevels = SpellManager.Instance.GetSpellLevels(spellId).ToArray();

            if (spellLevels.Length == 0)
            {
                logger.Error($"Cannot apply fix on spell {spellId} : spell do not exists");
                return;
            }

            foreach (var level in spellLevels)
            {
                foreach (var spellEffect in level.Effects.Where(entry => predicate(entry)))
                {
                    fixer(level, spellEffect, false);
                }

                if (!critical)
                    continue;

                foreach (var spellEffect in level.CriticalEffects.Where(entry => predicate(entry)))
                {
                    fixer(level, spellEffect, true);
                }
            }
        }

        public static void FixEffectOnLevel(int spellId, int level, int effectIndex, Action<SpellLevelTemplate, EffectDice, bool> fixer, bool critical = true)
        {
            var spellLevels = SpellManager.Instance.GetSpellLevels(spellId).ToArray();

            if (spellLevels.Length == 0)
                throw new Exception($"Cannot apply fix on spell {spellId} : spell do not exists");

            var spell = spellLevels[level - 1];

            fixer(spell, spell.Effects[effectIndex], false);
            if (critical && spell.CriticalEffects.Count > effectIndex)
                fixer(spell, spell.CriticalEffects[effectIndex], true);
        }

        public static void RemoveEffectOnAllLevels(int spellId, int effectIndex, bool critical = true)
        {
            var spellLevels = SpellManager.Instance.GetSpellLevels(spellId).ToArray();

            if (spellLevels.Length == 0)
            {
                logger.Error($"Cannot apply fix on spell {spellId} : spell do not exists");
                return;
            }

            foreach (var level in spellLevels)
            {
                level.Effects.RemoveAt(effectIndex);
                if (critical)
                    level.CriticalEffects.RemoveAt(effectIndex);

            }
        }

        public static void RemoveEffectOnAllLevels(int spellId, EffectsEnum effect, bool critical = true)
        {
            var spellLevels = SpellManager.Instance.GetSpellLevels(spellId).ToArray();

            if (spellLevels.Length == 0)
            {
                logger.Error($"Cannot apply fix on spell {spellId} : spell do not exists");
                return;
            }

            foreach (var level in spellLevels)
            {
                level.Effects.RemoveAll(entry => entry.EffectId == effect);
                if (critical)
                    level.CriticalEffects.RemoveAll(entry => entry.EffectId == effect);
            }
        }

        #endregion Methods
    }
}