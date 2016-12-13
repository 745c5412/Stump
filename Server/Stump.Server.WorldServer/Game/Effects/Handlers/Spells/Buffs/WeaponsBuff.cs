using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Buffs
{
    [EffectHandler(EffectsEnum.Effect_AddDamageBonusPercent)]
    public class WeaponsBuff : SpellEffectHandler
    {
        public WeaponsBuff(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            foreach (var actor in GetAffectedActors())
            {
                var weapon = (actor as CharacterFighter)?.Character.Inventory.TryGetItem(CharacterInventoryPositionEnum.ACCESSORY_POSITION_WEAPON);

                if (weapon == null || Dice.DiceNum != weapon.Template.TypeId)
                    continue;

                var id = actor.PopNextBuffId();
                var buff = new WeaponBuff(id, actor, Caster, Effect, Spell, Dice, Critical, true);

                actor.AddAndApplyBuff(buff);
            }

            return true;
        }
    }
}