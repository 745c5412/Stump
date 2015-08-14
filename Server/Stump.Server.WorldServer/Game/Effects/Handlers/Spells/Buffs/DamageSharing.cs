﻿using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs.Customs;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Buffs
{
    [EffectHandler(EffectsEnum.Effect_DamageSharing)]
    public class DamageSharing : SpellEffectHandler
    {
        public DamageSharing(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            var actors = GetAffectedActors(x => x is CharacterFighter && x.IsFriendlyWith(Caster)).ToArray();

            if (actors.Count() <= 1)
                return false;

            foreach (var actor in actors)
            {
                var buffId = actor.PopNextBuffId();
                var buff = new FractionBuff(buffId, actor, Caster, Dice, Spell, Critical, true, actors);

                actor.AddAndApplyBuff(buff);
            }

            return true;
        }
    }
}