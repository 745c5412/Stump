﻿using Stump.Server.WorldServer.Game.Actors.Fight;

{
    public class TelefragCriterion : TargetCriterion
    {
        public override bool CheckWhenExecute => true;

        public override bool IsTargetValid(FightActor actor, SpellEffectHandler handler)
            => actor.NeedTelefragState;
    }
}