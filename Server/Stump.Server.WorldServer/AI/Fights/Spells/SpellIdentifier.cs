﻿#region License GNU GPL
// SpellIdentifier.cs
// 
// Copyright (C) 2013 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
#endregion

using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.AI.Fights.Spells
{
    public class SpellIdentifier
    {
        public static SpellCategory GetSpellCategories(Spell spell)
        {
            return spell.CurrentSpellLevel.Effects.Aggregate(SpellCategory.None, (current, effect) => current | GetEffectCategories(effect.EffectId));
        }

        public static SpellCategory GetEffectCategories(EffectsEnum effectId)
        {
            switch (effectId)
            {
                case EffectsEnum.Effect_StealHPAir:
                    return SpellCategory.DamagesAir | SpellCategory.Healing;
                case EffectsEnum.Effect_StealHPWater:
                    return SpellCategory.DamagesWater | SpellCategory.Healing;
                case EffectsEnum.Effect_StealHPFire:
                    return SpellCategory.DamagesFire | SpellCategory.Healing;
                case EffectsEnum.Effect_StealHPEarth:
                    return SpellCategory.DamagesEarth | SpellCategory.Healing;
                case EffectsEnum.Effect_StealHPNeutral:
                    return SpellCategory.DamagesNeutral | SpellCategory.Healing;
                case EffectsEnum.Effect_DamageFire:
                    return SpellCategory.DamagesFire;
                case EffectsEnum.Effect_DamageWater:
                    return SpellCategory.DamagesWater;
                case EffectsEnum.Effect_DamageAir:
                    return SpellCategory.DamagesAir;
                case EffectsEnum.Effect_DamageNeutral:
                case EffectsEnum.Effect_Punishment_Damage:
                    return SpellCategory.DamagesNeutral;
                case EffectsEnum.Effect_DamageEarth:
                    return SpellCategory.DamagesEarth;
                case EffectsEnum.Effect_HealHP_108:
                case EffectsEnum.Effect_HealHP_143:
                case EffectsEnum.Effect_HealHP_81:
                    return SpellCategory.Healing;
                case EffectsEnum.Effect_Kill:
                    return SpellCategory.Damages;
                case EffectsEnum.Effect_Summon:
                case EffectsEnum.Effect_Double:
                case EffectsEnum.Effect_185:
                case EffectsEnum.Effect_621:
                case EffectsEnum.Effect_623:
                    return SpellCategory.Summoning;
                case EffectsEnum.Effect_AddArmorDamageReduction:
                case EffectsEnum.Effect_AddAirResistPercent:
                case EffectsEnum.Effect_AddFireResistPercent:
                case EffectsEnum.Effect_AddEarthResistPercent:
                case EffectsEnum.Effect_AddWaterResistPercent:
                case EffectsEnum.Effect_AddNeutralResistPercent:
                case EffectsEnum.Effect_AddAirElementReduction:
                case EffectsEnum.Effect_AddFireElementReduction:
                case EffectsEnum.Effect_AddEarthElementReduction:
                case EffectsEnum.Effect_AddWaterElementReduction:
                case EffectsEnum.Effect_AddNeutralElementReduction:
                case EffectsEnum.Effect_AddAgility:
                case EffectsEnum.Effect_AddStrength:
                case EffectsEnum.Effect_AddIntelligence:
                case EffectsEnum.Effect_AddHealth:
                case EffectsEnum.Effect_AddChance:
                case EffectsEnum.Effect_AddCriticalHit:
                case EffectsEnum.Effect_AddCriticalDamageBonus:
                case EffectsEnum.Effect_AddCriticalDamageReduction:
                case EffectsEnum.Effect_AddDamageBonus:
                case EffectsEnum.Effect_AddDamageBonusPercent:
                case EffectsEnum.Effect_AddDamageBonus_121:
                case EffectsEnum.Effect_AddFireDamageBonus:
                case EffectsEnum.Effect_AddAirDamageBonus:
                case EffectsEnum.Effect_AddWaterDamageBonus:
                case EffectsEnum.Effect_AddEarthDamageBonus:
                case EffectsEnum.Effect_AddNeutralDamageBonus:
                case EffectsEnum.Effect_AddDamageMultiplicator:
                case EffectsEnum.Effect_AddDamageReflection:
                case EffectsEnum.Effect_AddGlobalDamageReduction:
                case EffectsEnum.Effect_AddGlobalDamageReduction_105:
                case EffectsEnum.Effect_AddAP_111:
                case EffectsEnum.Effect_AddHealBonus:
                case EffectsEnum.Effect_AddWisdom:
                case EffectsEnum.Effect_AddProspecting:
                case EffectsEnum.Effect_AddMP:
                case EffectsEnum.Effect_AddMP_128:
                case EffectsEnum.Effect_AddPhysicalDamage_137:
                case EffectsEnum.Effect_AddPhysicalDamage_142:
                case EffectsEnum.Effect_AddPhysicalDamageReduction:
                case EffectsEnum.Effect_AddPushDamageReduction:
                case EffectsEnum.Effect_AddPushDamageBonus:
                case EffectsEnum.Effect_AddRange:
                case EffectsEnum.Effect_AddRange_136:
                case EffectsEnum.Effect_AddSummonLimit:
                case EffectsEnum.Effect_AddVitality:
                case EffectsEnum.Effect_AddVitalityPercent:
                case EffectsEnum.Effect_Dodge:
                case EffectsEnum.Effect_IncreaseAPAvoid:
                case EffectsEnum.Effect_IncreaseMPAvoid:
                case EffectsEnum.Effect_Invisibility:
                case EffectsEnum.Effect_ReflectSpell:
                case EffectsEnum.Effect_RegainAP:
                    return SpellCategory.Buff;
                case EffectsEnum.Effect_Teleport:
                    return SpellCategory.Teleport;
                case EffectsEnum.Effect_PushBack:
                case EffectsEnum.Effect_RemoveAP:
                case EffectsEnum.Effect_LostMP:
                case EffectsEnum.Effect_StealKamas:
                case EffectsEnum.Effect_LoseHPByUsingAP:
                case EffectsEnum.Effect_LosingAP:
                case EffectsEnum.Effect_LosingMP:
                case EffectsEnum.Effect_SubRange_135:
                case EffectsEnum.Effect_SkipTurn:
                case EffectsEnum.Effect_SubDamageBonus:
                case EffectsEnum.Effect_SubChance:
                case EffectsEnum.Effect_SubVitality:
                case EffectsEnum.Effect_SubAgility:
                case EffectsEnum.Effect_SubIntelligence:
                case EffectsEnum.Effect_SubWisdom:
                case EffectsEnum.Effect_SubStrength:
                case EffectsEnum.Effect_SubDodgeAPProbability:
                case EffectsEnum.Effect_SubDodgeMPProbability:
                case EffectsEnum.Effect_SubAP:
                case EffectsEnum.Effect_SubMP:
                case EffectsEnum.Effect_SubCriticalHit:
                case EffectsEnum.Effect_SubMagicDamageReduction:
                case EffectsEnum.Effect_SubPhysicalDamageReduction:
                case EffectsEnum.Effect_SubInitiative:
                case EffectsEnum.Effect_SubProspecting:
                case EffectsEnum.Effect_SubHealBonus:
                case EffectsEnum.Effect_SubDamageBonusPercent:
                case EffectsEnum.Effect_197:
                case EffectsEnum.Effect_SubEarthResistPercent:
                case EffectsEnum.Effect_SubWaterResistPercent:
                case EffectsEnum.Effect_SubAirResistPercent:
                case EffectsEnum.Effect_SubFireResistPercent:
                case EffectsEnum.Effect_SubNeutralResistPercent:
                case EffectsEnum.Effect_SubEarthElementReduction:
                case EffectsEnum.Effect_SubWaterElementReduction:
                case EffectsEnum.Effect_SubAirElementReduction:
                case EffectsEnum.Effect_SubFireElementReduction:
                case EffectsEnum.Effect_SubNeutralElementReduction:
                case EffectsEnum.Effect_SubPvpEarthResistPercent:
                case EffectsEnum.Effect_SubPvpWaterResistPercent:
                case EffectsEnum.Effect_SubPvpAirResistPercent:
                case EffectsEnum.Effect_SubPvpFireResistPercent:
                case EffectsEnum.Effect_SubPvpNeutralResistPercent:
                case EffectsEnum.Effect_StealChance:
                case EffectsEnum.Effect_StealVitality:
                case EffectsEnum.Effect_StealAgility:
                case EffectsEnum.Effect_StealIntelligence:
                case EffectsEnum.Effect_StealWisdom:
                case EffectsEnum.Effect_StealStrength:
                case EffectsEnum.Effect_275:
                case EffectsEnum.Effect_276:
                case EffectsEnum.Effect_277:
                case EffectsEnum.Effect_278:
                case EffectsEnum.Effect_279:
                case EffectsEnum.Effect_411:
                case EffectsEnum.Effect_413:
                case EffectsEnum.Effect_SubCriticalDamageBonus:
                case EffectsEnum.Effect_SubPushDamageReduction:
                case EffectsEnum.Effect_SubCriticalDamageReduction:
                case EffectsEnum.Effect_SubEarthDamageBonus:
                case EffectsEnum.Effect_SubFireDamageBonus:
                case EffectsEnum.Effect_SubWaterDamageBonus:
                case EffectsEnum.Effect_SubAirDamageBonus:
                case EffectsEnum.Effect_SubNeutralDamageBonus:
                case EffectsEnum.Effect_StealAP_440:
                case EffectsEnum.Effect_StealMP_441:
                case EffectsEnum.Effect_StealMP_77:
                    return SpellCategory.Curse;
            }
            return SpellCategory.None;

        }
    }
}