﻿using NLog;
using Stump.Core.Attributes;
using Stump.Core.Collections;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Spells;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Stump.Server.WorldServer.Game.Fights.History
{
    public class SpellHistory
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        [Variable]
        public static readonly int HistoryEntriesLimit = 60;

        private readonly LimitedStack<SpellHistoryEntry> m_underlyingStack = new LimitedStack<SpellHistoryEntry>(HistoryEntriesLimit);

        public SpellHistory(FightActor owner)
        {
            Owner = owner;
        }

        public SpellHistory(FightActor owner, IEnumerable<SpellHistoryEntry> entries)
        {
            Owner = owner;
            m_underlyingStack = new LimitedStack<SpellHistoryEntry>(HistoryEntriesLimit, entries);
        }

        public FightActor Owner
        {
            get;
            private set;
        }

        private int CurrentRound
        {
            get { return Owner.Fight.TimeLine.RoundNumber; }
        }

        public IEnumerable<SpellHistoryEntry> GetEntries()
        {
            return m_underlyingStack;
        }

        public IEnumerable<SpellHistoryEntry> GetEntries(Predicate<SpellHistoryEntry> predicate)
        {
            return m_underlyingStack.Where(entry => predicate(entry));
        }

        public void RegisterCastedSpell(SpellHistoryEntry entry)
        {
            m_underlyingStack.Push(entry);
        }

        public void RegisterCastedSpell(SpellLevelTemplate spell, FightActor target)
        {
            RegisterCastedSpell(new SpellHistoryEntry(this, spell, Owner, target, CurrentRound, 0));
        }

        public bool CanCastSpell(SpellLevelTemplate spell, Cell targetedCell)
        {
            if (!CanCastSpell(spell))
                return false;

            var target = Owner.Fight.GetOneFighter(targetedCell);

            if (target == null)
                return true;

            var castsThisRound = m_underlyingStack.Where(entry => entry.Spell.Id == spell.Id && entry.CastRound == CurrentRound).ToArray();
            var castsOnThisTarget = castsThisRound.Count(entry => entry.Target != null && entry.Target.Id == target.Id);

            return spell.MaxCastPerTarget <= 0 || castsOnThisTarget < spell.MaxCastPerTarget;
        }

        public bool CanCastSpell(SpellLevelTemplate spell)
        {
            var mostRecentEntry = m_underlyingStack.LastOrDefault(entry => entry.Spell.Id == spell.Id);

            //check initial cooldown
            if (mostRecentEntry == null && CurrentRound < spell.InitialCooldown)
            {
                return false;
            }

            if (mostRecentEntry == null)
                return true;

            if (mostRecentEntry.IsGlobalCooldownActive(CurrentRound))
            {
                return false;
            }
            var castsThisRound = m_underlyingStack.Where(entry => entry.Spell.Id == spell.Id && entry.CastRound == CurrentRound).ToArray();

            if (castsThisRound.Length == 0)
                return true;

            if (spell.MaxCastPerTurn > 0 && castsThisRound.Length >= spell.MaxCastPerTurn)
            {
                return false;
            }

            return true;
        }

        public int GetSpellCooldown(SpellLevelTemplate spell)
        {
            var mostRecentEntry = m_underlyingStack.LastOrDefault(entry => entry.Spell.Id == spell.Id);

            if (mostRecentEntry == null && CurrentRound < spell.InitialCooldown)
            {
                return (int)(spell.InitialCooldown - CurrentRound);
            }

            if (mostRecentEntry == null)
                return 0;

            var elapsedCd = mostRecentEntry.GetElapsedRounds(CurrentRound);

            if (elapsedCd < mostRecentEntry.CooldownDuration)
            {
                return mostRecentEntry.CooldownDuration - elapsedCd;
            }

            var castsThisRound = m_underlyingStack.Where(entry => entry.Spell.Id == spell.Id && entry.CastRound == CurrentRound).ToArray();

            if (castsThisRound.Length == 0)
                return 0;

            if (spell.MaxCastPerTurn > 0 && castsThisRound.Length >= spell.MaxCastPerTurn)
            {
                return 1;
            }

            return 0;
        }

        public GameFightSpellCooldown[] GetCooldowns()
        {
            var spells = m_underlyingStack.Select(x => x.Spell).Distinct();

            return (from spell in spells
                    let cd = GetSpellCooldown(spell)
                    where cd > 0
                    select new GameFightSpellCooldown((int)spell.SpellId, (sbyte)cd)).ToArray();
        }
    }
}