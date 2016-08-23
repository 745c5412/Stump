﻿using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace Stump.Server.WorldServer.Database.Misc
{
    public class AutoAnnounceMessageRelator
    {
        public static string FecthQuery = "SELECT * FROM announces";
    }

    [TableName("announces")]
    public class AutoAnnounceMessage : AutoAssignedRecord<AutoAnnounceMessage>, IAutoGeneratedRecord
    {
        public string Message
        {
            get;
            set;
        }

        public int? Color
        {
            get;
            set;
        }
    }
}