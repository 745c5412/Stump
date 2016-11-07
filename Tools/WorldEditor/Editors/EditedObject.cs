﻿#region License GNU GPL

// D2OEditedObject.cs
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

using System.ComponentModel;
using WorldEditor.Editors.Files.D2O;

namespace WorldEditor.Editors
{
    public class EditedObject : INotifyPropertyChanged
    {
        public EditedObject(object o, ObjectState state)
        {
            Object = o;
            State = state;
        }

        public object Object
        {
            get;
            private set;
        }

        public ObjectState State
        {
            get;
            set;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}