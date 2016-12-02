using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;
using System;

namespace Stump.Server.WorldServer.Game.Fights.Buffs
{
    public class WeaponBuff : Buff
    {
        public WeaponBuff(int id, FightActor target, FightActor caster, EffectBase effect, Spell spell, EffectDice dice, bool critical, bool dispelable)
            : base(id, target, caster, effect, spell, critical, dispelable)
        {
            Dice = dice;
        }

        EffectDice Dice
        {
            get;
        }

        public override void Apply()
        {

            Target.Stats[PlayerFields.WeaponDamageBonusPercent].Context += Dice.DiceFace;
        }

        public override void Dispell()
        {
            if (!Target.IsAlive())
                return;

            Target.Stats[PlayerFields.WeaponDamageBonusPercent].Context -= Dice.DiceFace;

            Target.CheckDead(Target);
        }

        public override AbstractFightDispellableEffect GetAbstractFightDispellableEffect() => new FightTemporaryBoostWeaponDamagesEffect(Id, Target.Id, Duration, (sbyte)(Dispellable ? 0 : 1), (short)Spell.Id, 0, Math.Abs(Dice.DiceFace), Dice.DiceNum);
    }
}