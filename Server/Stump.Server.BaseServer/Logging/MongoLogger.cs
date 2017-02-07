﻿using MongoDB.Bson;
using MongoDB.Driver;
using Stump.Core.Attributes;
using Stump.Core.Reflection;
using Stump.Core.Threading;
using Stump.ORM;
using Stump.Server.BaseServer.Initialization;
using System;

namespace Stump.Server.BaseServer.Logging
{
    public class MongoLogger : Singleton<MongoLogger>
    {
        [Variable(Priority = 10, DefinableRunning = true)]
        public static bool IsMongoLoggerEnabled;

        [Variable(Priority = 10, DefinableRunning = true)]
        public static DatabaseConfiguration MongoDBConfiguration = new DatabaseConfiguration
        {
            Host = "localhost",
            DbName = "stump_logs",
            Port = "3307",
            User = "root",
            Password = ""
        };

        private IMongoDatabase m_database;

        [Initialization(InitializationPass.Fourth)]
        public void Initialize()
        {
            if (!IsMongoLoggerEnabled)
                return;

            var client = new MongoClient(String.Format("mongodb://{0}:{1}@{2}:{3}/{4}{5}",
                MongoDBConfiguration.User, MongoDBConfiguration.Password, MongoDBConfiguration.Host, MongoDBConfiguration.Port, MongoDBConfiguration.DbName,
                string.IsNullOrEmpty(MongoDBConfiguration.Password) ? "" : "?authMechanism=SCRAM-SHA-1"));

            m_database = client.GetDatabase(MongoDBConfiguration.DbName);
        }

        public bool Insert(string collection, BsonDocument document)
        {
            if (!IsMongoLoggerEnabled)
            {
                if (m_database == null)
                    return false;

                m_database = null;

                return false;
            }

            if (m_database == null)
                Initialize();

            if (m_database != null)
                m_database.GetCollection<BsonDocument>(collection).InsertOneAsync(document);
            else
                return false;

            return true;
        }
    }
}