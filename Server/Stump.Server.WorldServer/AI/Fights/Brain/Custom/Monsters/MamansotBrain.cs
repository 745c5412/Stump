﻿using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.AI.Fights.Brain.Custom.Monsters
{
    [BrainIdentifier(2857)]
    public class MamansotBrain : Brain
    {
        public MamansotBrain(AIFighter fighter)
            : base(fighter)
        {
            SpellSelector.AnalysePossibilitiesFinished += OnAnalysePossibilitiesFinished;
        }

        private void OnAnalysePossibilitiesFinished(AIFighter obj)
        {
            if (Fighter.Team.GetLastDeadFighter() == null)
                SpellSelector.Possibilities.RemoveAll(x => x.Spell.Id == (int)SpellIdEnum.MANSOVEGARDE);
        }
    }
}