﻿#region License GNU GPL

// ColorByOperatorConverter.cs
//
// Copyright (C) 2012 - BehaviorIsManaged
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

#endregion License GNU GPL

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace WorldEditor.Search.Items
{
    public class ColorByOperatorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var @operator = value as string;
            if (@operator == null)
                return Brushes.Black;

            if (@operator == "+")
                return Brushes.Green;

            if (@operator == "-")
                return Brushes.Red;

            return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}