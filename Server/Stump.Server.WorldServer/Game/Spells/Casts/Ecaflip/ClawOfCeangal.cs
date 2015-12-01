﻿using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights.Buffs;

namespace Stump.Server.WorldServer.Game.Spells.Casts.Ecaflip
{
    [SpellCastHandler(SpellIdEnum.GRIFFE_DE_CEANGAL)]
    [SpellCastHandler(SpellIdEnum.GRIFFE_DE_CEANGAL_DU_DOPEUL)]
    public class ClawOfCeangal : DefaultSpellCastHandler
    {
        public ClawOfCeangal(FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(caster, spell, targetedCell, critical)
        {
        }

        public override void Execute()
        {
            if (!m_initialized)
                Initialize();

            var damageHandler = Handlers[1];

            if (damageHandler == null)
                return;

            var affectedActor = damageHandler.GetAffectedActors().FirstOrDefault();

            if (affectedActor == null)
                return;

            damageHandler.Apply(); //Damages
            Handlers[0].AddTriggerBuff(affectedActor, true, BuffTriggerType.AFTER_HEALED, BuffTrigger);
        }

        void BuffTrigger(TriggerBuff buff, BuffTriggerType trigger, object token)
        {
            Handlers[0].Apply();
        }
    }
}