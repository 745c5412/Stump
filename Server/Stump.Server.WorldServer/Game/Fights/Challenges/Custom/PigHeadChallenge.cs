﻿using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier((int)ChallengeEnum.BORNÉ)]
    public class PigHeadChallenge : DefaultChallenge
    {
        public PigHeadChallenge(int id, IFight fight)
            : base(id, fight)
        {
            Bonus = 50;

            foreach (var fighter in Fight.GetAllFighters<CharacterFighter>())
            {
                fighter.SpellCasting += OnSpellCasting;
            }
        }

        private void OnSpellCasting(FightActor caster, Spell spell, Cell target, FightSpellCastCriticalEnum critical, bool silentCast)
        {
            var entries = caster.SpellHistory.GetEntries().ToArray();
            if (!entries.Any())
                return;

            var result = entries.Where(x => x.Spell.SpellId == spell.Id);

            if (result.Any())
                return;

            UpdateStatus(ChallengeStatusEnum.FAILED, caster);
        }
    }
}
