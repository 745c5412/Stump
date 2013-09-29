﻿#region License GNU GPL
// ObjectDataManager.cs
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
#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NLog;
using Stump.Core.Reflection;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using WorldEditor.Config;

namespace WorldEditor.Loaders.D2O
{
    /// <summary>
    /// Retrieves D2O objects. Thread safe
    /// </summary>
    public class ObjectDataManager : Singleton<ObjectDataManager>
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly Dictionary<Type, D2OReader> m_readers = new Dictionary<Type, D2OReader>();
        private readonly Dictionary<Type, D2OWriter> m_writers = new Dictionary<Type, D2OWriter>();
        private readonly List<Type> m_ignoredTyes = new List<Type>();

        public void AddReaders(string directory)
        {
            foreach (var reader in Directory.EnumerateFiles(directory).Where(entry => entry.EndsWith(".d2o")).Select(d2oFile => new D2OReader(d2oFile)))
            {
                AddReader(reader);
            }
        }

        public void AddReader(D2OReader d2oFile)
        {
            var classes = d2oFile.Classes;

            foreach (var @class in classes)
            {
                if (m_ignoredTyes.Contains(@class.Value.ClassType))
                    continue;

                if (m_readers.ContainsKey(@class.Value.ClassType))
                {
                    // this classes are not bound to a single file, so we ignore them
                    m_ignoredTyes.Add(@class.Value.ClassType);
                    m_readers.Remove(@class.Value.ClassType);
                }
                else
                {
                    m_readers.Add(@class.Value.ClassType, d2oFile);
                    m_writers.Add(@class.Value.ClassType, new D2OWriter(d2oFile.FilePath));
                }
            }

            logger.Info("File added : {0}", Path.GetFileName(d2oFile.FilePath));

        }

        public T Get<T>(uint key)
            where T : class
        {
            return Get<T>((int)key);
        }

        public T Get<T>(int key)
            where T : class
        {
            if (!m_readers.ContainsKey(typeof(T))) // This exception should be called in all cases (serious)
                throw new ArgumentException("Cannot find data corresponding to type : " + typeof(T));

            var reader = m_readers[typeof(T)];

            return reader.ReadObject<T>(key, true);
        }

        public T GetOrDefault<T>(uint key)
            where T : class
        {
            return GetOrDefault<T>((int)key);
        }

        public T GetOrDefault<T>(int key)
            where T : class
        {
            try
            {
                return Get<T>(key);
            }
            catch
            {
                return null;
            }
        }

        public void StartEditing<T>()
        {
            if (!m_writers.ContainsKey(typeof(T))) // This exception should be called in all cases (serious)
                throw new ArgumentException("Cannot find data corresponding to type : " + typeof(T));

            var writer = m_writers[typeof(T)];
            writer.StartWriting();
        }

        public void EndEditing<T>()
        {
            if (!m_writers.ContainsKey(typeof(T))) // This exception should be called in all cases (serious)
                throw new ArgumentException("Cannot find data corresponding to type : " + typeof(T));

            var writer = m_writers[typeof(T)];
            writer.EndWriting();

            // reset reader
            var reader = new D2OReader(writer.Filename);
            m_readers[typeof (T)] = reader;
        }

        public void Set<T>(uint key, T value)
            where T : class
        {
            Set((int)key, value);
        }

        public void Set<T>(int key, T value)
        {
            if (!m_writers.ContainsKey(typeof(T))) // This exception should be called in all cases (serious)
                throw new ArgumentException("Cannot find data corresponding to type : " + typeof(T));

            var writer = m_writers[typeof(T)];
            writer.Write(value, key);
        }

        public void Remove<T>(int key)
        {
            if (!m_writers.ContainsKey(typeof(T))) // This exception should be called in all cases (serious)
                throw new ArgumentException("Cannot find data corresponding to type : " + typeof(T));

            var writer = m_writers[typeof(T)];
            writer.Delete(key);
        }

        public int FindFreeId<T>()
        {
            if (!m_writers.ContainsKey(typeof(T))) // This exception should be called in all cases (serious)
                throw new ArgumentException("Cannot find data corresponding to type : " + typeof(T));

            var maxId = m_readers[typeof (T)].FindFreeId();

            return maxId < Settings.MinDataId ? Settings.MinDataId : maxId;
        }

        public IEnumerable<Type> GetAllTypes()
        {
            return m_readers.Keys;
        }

        public IEnumerable<object> EnumerateObjects(Type type)
        {
            if (!m_readers.ContainsKey(type))
                throw new ArgumentException("Cannot find data corresponding to type : " + type);

            var reader = m_readers[type];

            return reader.Indexes.Select(index => reader.ReadObject(index.Key, true)).Where(obj=>obj.GetType().Name == type.Name);
        }

        public IEnumerable<T> EnumerateObjects<T>() where T : class
        {
            if (!m_readers.ContainsKey(typeof(T)))
                throw new ArgumentException("Cannot find data corresponding to type : " + typeof(T));

            var reader = m_readers[typeof(T)];

            return reader.Indexes.Select(index => reader.ReadObject(index.Key, true)).OfType<T>().Select(obj => obj);
        }
    }
}