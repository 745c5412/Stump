﻿using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Move
{
    [EffectHandler(EffectsEnum.Effect_SwitchPosition)]
    [EffectHandler(EffectsEnum.Effect_SwitchPosition_1023)]
    public class SwitchPosition : SpellEffectHandler
    {
        public SwitchPosition(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            var target = GetAffectedActors().FirstOrDefault();

            if (target == null)
                return false;

            if (target.HasState((int) SpellStatesEnum.Unmovable))
                return false;

            if (target.IsCarrying())
                return false;

            Caster.ExchangePositions(target);

            target.OnActorMoved(Caster, false);
            Caster.OnActorMoved(Caster, false);

            return true;
        }
    }
}