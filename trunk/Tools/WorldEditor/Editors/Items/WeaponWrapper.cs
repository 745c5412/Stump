﻿#region License GNU GPL

// WeaponWrapper.cs
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

using System;
using System.Collections.ObjectModel;
using System.Linq;
using Stump.DofusProtocol.D2oClasses;

namespace WorldEditor.Editors.Items
{
    public class WeaponWrapper : ItemWrapper
    {
        private Weapon m_wrappedWeapon;

        public WeaponWrapper(Weapon wrappedWeapon)
            : base(wrappedWeapon)
        {
            WrappedWeapon = wrappedWeapon;
        }

        public WeaponWrapper(ItemWrapper item)
        {
            var weapon = new Weapon();
            weapon.id = item.WrappedItem.id;
            weapon.nameId = item.WrappedItem.nameId;
            weapon.typeId = item.WrappedItem.typeId;
            weapon.descriptionId = item.WrappedItem.descriptionId;
            weapon.iconId = item.WrappedItem.iconId;
            weapon.level = item.WrappedItem.level;
            weapon.realWeight = item.WrappedItem.realWeight;
            weapon.cursed = item.WrappedItem.cursed;
            weapon.useAnimationId = item.WrappedItem.useAnimationId;
            weapon.usable = item.WrappedItem.usable;
            weapon.targetable = item.WrappedItem.targetable;
            weapon.price = item.WrappedItem.price;
            weapon.twoHanded = item.WrappedItem.twoHanded;
            weapon.etheral = item.WrappedItem.etheral;
            weapon.itemSetId = item.WrappedItem.itemSetId;
            weapon.criteria = item.WrappedItem.criteria;
            weapon.criteriaTarget = item.WrappedItem.criteriaTarget;
            weapon.hideEffects = item.WrappedItem.hideEffects;
            weapon.enhanceable = item.WrappedItem.enhanceable;
            weapon.nonUsableOnAnother = item.WrappedItem.nonUsableOnAnother;
            weapon.appearanceId = item.WrappedItem.appearanceId;
            weapon.secretRecipe = item.WrappedItem.secretRecipe;
            weapon.recipeIds = item.WrappedItem.recipeIds;
            weapon.bonusIsSecret = item.WrappedItem.bonusIsSecret;
            weapon.possibleEffects = item.WrappedItem.possibleEffects;
            weapon.favoriteSubAreas = item.WrappedItem.favoriteSubAreas;
            weapon.favoriteSubAreasBonus = item.WrappedItem.favoriteSubAreasBonus;
            weapon.type = item.WrappedItem.type;
            weapon.weight = item.WrappedItem.weight;
            WrappedItem = WrappedWeapon = weapon;

            Name = item.Name;
            Description = item.Description;

            m_effects = new ObservableCollection<EffectWrapper>(PossibleEffects.Select(EffectWrapper.Create));
            m_effects.CollectionChanged += OnEffectsChanged;
        }

        public Weapon WrappedWeapon
        {
            get;
            private set;
        }

        public int ApCost
        {
            get { return WrappedWeapon.apCost; }
            set { WrappedWeapon.apCost = value; }
        }

        public int MinRange
        {
            get { return WrappedWeapon.minRange; }
            set { WrappedWeapon.minRange = value; }
        }

        public int Range
        {
            get { return WrappedWeapon.range; }
            set { WrappedWeapon.range = value; }
        }

        public Boolean CastInLine
        {
            get { return WrappedWeapon.castInLine; }
            set { WrappedWeapon.castInLine = value; }
        }

        public Boolean CastInDiagonal
        {
            get { return WrappedWeapon.castInDiagonal; }
            set { WrappedWeapon.castInDiagonal = value; }
        }

        public Boolean CastTestLos
        {
            get { return WrappedWeapon.castTestLos; }
            set { WrappedWeapon.castTestLos = value; }
        }

        public int CriticalHitProbability
        {
            get { return WrappedWeapon.criticalHitProbability; }
            set { WrappedWeapon.criticalHitProbability = value; }
        }

        public int CriticalHitBonus
        {
            get { return WrappedWeapon.criticalHitBonus; }
            set { WrappedWeapon.criticalHitBonus = value; }
        }

        public int CriticalFailureProbability
        {
            get { return WrappedWeapon.criticalFailureProbability; }
            set { WrappedWeapon.criticalFailureProbability = value; }
        }
    }
}