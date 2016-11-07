﻿using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Damage;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using System.Linq;

namespace Stump.Server.WorldServer.Game.Spells.Casts.Ecaflip
{
    [SpellCastHandler(SpellIdEnum.ESPRIT_FÉLIN)]
    public class FelineSpiritCastHandler : DefaultSpellCastHandler
    {
        public FelineSpiritCastHandler(FightActor caster, Spell spell, Cell targetedCell, bool critical) : base(caster, spell, targetedCell, critical)
        {
        }

        public override bool Initialize()
        {
            base.Initialize();

            var damageHandler = (Handlers[1] as DirectDamage);

            if (damageHandler != null)
                damageHandler.BuffTriggerType = BuffTriggerType.BUFF_ENDED_TURNEND;

            return true;
        }

        public override void Execute()
        {
            var damageBuffs = Caster.GetBuffs(entry => entry.Spell == Spell).ToArray();

            foreach (var buff in damageBuffs)
            {
                Caster.RemoveBuff(buff);
            }

            base.Execute();
        }
    }
}