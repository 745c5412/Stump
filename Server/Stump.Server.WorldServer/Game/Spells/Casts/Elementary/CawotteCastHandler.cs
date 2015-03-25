﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Fights.Triggers;

namespace Stump.Server.WorldServer.Game.Spells.Casts.Elementary
{
    [SpellCastHandler(SpellIdEnum.CAWOTTE)]
    public class CawotteCastHandler : DefaultSpellCastHandler
    {
        public CawotteCastHandler(FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(caster, spell, targetedCell, critical)
        {
        }

        public override void Execute()
        {
            if (!m_initialized)
                Initialize();

            var summonEffect = Handlers[0];
            var glyphEffect = Handlers[1];

            if (summonEffect == null || glyphEffect == null)
                return;

            summonEffect.Apply();

            var cawotte = Caster.Summons.FirstOrDefault(x => x is SummonedMonster && ((SummonedMonster)x).Monster.MonsterId == summonEffect.Dice.DiceNum);

            if (cawotte == null)
                return;

            var stateRooted = SpellManager.Instance.GetSpellState((uint)SpellStatesEnum.Rooted);

            var stateBuff = new StateBuff(cawotte.PopNextBuffId(), cawotte, cawotte, summonEffect.Effect, Spell, false, stateRooted)
            {
                Duration = -1
            };

            cawotte.AddAndApplyBuff(stateBuff);

            glyphEffect.Apply();
        }
    }
}
