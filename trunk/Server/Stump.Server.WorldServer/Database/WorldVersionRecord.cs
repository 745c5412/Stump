using System;
using Castle.ActiveRecord;
using Stump.Server.BaseServer.Database;

namespace Stump.Server.WorldServer.Database
{
    [ActiveRecord("version")]
    public class WorldVersionRecord : WorldBaseRecord<WorldVersionRecord>, IVersionRecord
    {
        [PrimaryKey(PrimaryKeyType.Assigned, "Revision")]
        public uint Revision
        {
            get;
            set;
        }

        [Property("UpdateDate")]
        public DateTime UpdateDate
        {
            get;
            set;
        }
    }
}