﻿#region License GNU GPL
// ItemTypeIdToTypeNameConverter.cs
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
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using Stump.DofusProtocol.D2oClasses;
using WorldEditor.Loaders.D2O;
using WorldEditor.Loaders.I18N;

namespace WorldEditor.Helpers.Converters
{
    public class ItemTypeIdToTypeNameConverter : IValueConverter
    {
        private static bool m_initialized;
        private static Dictionary<int, ItemType> m_types;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!m_initialized)
                Initialize();

            ItemType type;
            if (m_types.TryGetValue((int)value, out type))
                return I18NDataManager.Instance.ReadText(type.NameId);

            return value.ToString();
        }

        private static void Initialize()
        {
            m_types = ObjectDataManager.Instance.EnumerateObjects<ItemType>().ToDictionary(x => x.Id);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}