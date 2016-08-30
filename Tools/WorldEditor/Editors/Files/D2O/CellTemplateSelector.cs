﻿#region License GNU GPL

// CellTemplateSelector.cs
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

#endregion License GNU GPL

using System;
using System.Windows;
using System.Windows.Controls;

namespace WorldEditor.Editors.Files.D2O
{
    public class CellTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Template
        {
            get;
            set;
        }

        public Type ExpectedType
        {
            get;
            set;
        }

        public DataTemplate DefaultTemplate
        {
            get;
            set;
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null)
                return DefaultTemplate;

            var type = item.GetType();

            if (type != ExpectedType && !type.IsSubclassOf(ExpectedType))
                return DefaultTemplate;

            return Template;
        }
    }
}