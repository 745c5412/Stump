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
using System;
using System.Reflection;

namespace Stump.BaseCore.Framework.Utils
{
    public abstract class Singleton<T> where T : class
    {
        /// <summary>
        ///   Returns the singleton instance.
        /// </summary>
        public static T Instance
        {
            get { return SingletonAllocator.instance; }
            protected set { SingletonAllocator.instance = value; }
        }

        #region Nested type: SingletonAllocator

        internal static class SingletonAllocator
        {
            internal static T instance;

            static SingletonAllocator()
            {
                CreateInstance(typeof (T));
            }

            public static T CreateInstance(Type type)
            {
                ConstructorInfo[] ctorsPublic = type.GetConstructors(
                    BindingFlags.Instance | BindingFlags.Public);

                if (ctorsPublic.Length > 0)
                    return instance = (T)Activator.CreateInstance(type);

                ConstructorInfo ctorNonPublic = type.GetConstructor(
                    BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[0], new ParameterModifier[0]);

                if (ctorNonPublic == null)
                {
                    throw new Exception(
                        type.FullName +
                        " doesn't have a private/protected constructor so the property cannot be enforced.");
                }

                try
                {
                    return instance = (T) ctorNonPublic.Invoke(new object[0]);
                }
                catch (Exception e)
                {
                    throw new Exception(
                        "The Singleton couldnt be constructed, check if " + type.FullName + " has a default constructor",
                        e);
                }
            }
        }

        #endregion
    }
}