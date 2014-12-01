﻿using System;
using System.Data;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;
using Stump.Server.WorldServer.Game.Mounts;

namespace Stump.Server.WorldServer.Database.Mounts
{
    public class MountRecordRelator
    {
        public static string FetchQuery = "SELECT * FROM mounts";
        /// <summary>
        /// Use string.Format
        /// </summary>
        public static string FindByCharacterId = "SELECT * WHERE OwnerId={0}";
    }

    [TableName("mounts")]
    public class MountRecord : IAutoGeneratedRecord
    {
        [PrimaryKey("Id", false)]
        public int Id
        {
            get;
            set;
        }

        public int OwnerId
        {
            get;
            set;
        }

        [Ignore]
        public bool IsNew
        {
            get;
            set;
        }

        public String Name
        {
            get;
            set;
        }

        public Boolean Sex
        {
            get;
            set;
        }

        public int ModelId
        {
            get;
            set;
        }

        [Ignore]
        public MountTemplate Model
        {
            get { return MountManager.Instance.GetTemplate(ModelId); }
        }

        public long Experience
        {
            get;
            set;
        }

        public sbyte GivenExperience
        {
            get;
            set;
        }

        public int Stamina
        {
            get;
            set;
        }

        public int Maturity
        {
            get;
            set;
        }

        public int Energy
        {
            get;
            set;
        }

        public int Serenity
        {
            get;
            set;
        }

        public int Love
        {
            get;
            set;
        }

        public int ReproductionCount
        {
            get;
            set;
        }
    }
}