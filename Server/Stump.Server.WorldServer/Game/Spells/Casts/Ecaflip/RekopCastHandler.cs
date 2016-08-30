﻿using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Damage;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using System;
using System.Linq;

namespace Stump.Server.WorldServer.Game.Spells.Casts.Ecaflip
{
    [SpellCastHandler(SpellIdEnum.REKOP)]
    [SpellCastHandler(SpellIdEnum.REKOP_DU_DOPEUL)]
    public class RekopCastHandler : DefaultSpellCastHandler
    {
        public RekopCastHandler(FightActor caster, Spell spell, Cell targetedCell, bool critical) : base(caster, spell, targetedCell, critical)
        {
        }

        public int CastRound
        {
            get;
            set;
        }

        public override bool Initialize()
        {
            base.Initialize();

            // 0 to 3 rounds
            CastRound = new Random().Next(0, 4);
            Handlers = Handlers.Where(entry => entry.Effect.Duration == CastRound).ToArray();

            foreach (var damageHandler in Handlers.OfType<DirectDamage>())
            {
                damageHandler.BuffTriggerType = BuffTriggerType.BUFF_ENDED;
            }

            return true;
        }
    }
}