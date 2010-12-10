﻿// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System.Collections.Generic;

namespace Stump.DofusProtocol.D2oClasses
{
    [AttributeAssociatedFile("Items")]
    public class Item
    {
        public int appearanceId;
        public bool bonusIsSecret;
        public string criteria;
        public bool cursed;
        public int descriptionId;
        public bool etheral;
        public List<int> favoriteSubAreas;
        public int favoriteSubAreasBonus;
        public bool hideEffects;
        public int iconId;
        public int id;
        public int itemSetId;
        public int level;
        public int nameId;
        public List<EffectInstance> possibleEffects;
        public int price;
        public List<int> recipeIds;
        public bool targetable;
        public bool twoHanded;
        public int typeId;
        public bool usable;
        public int useAnimationId;
        public int weight;
    }
}