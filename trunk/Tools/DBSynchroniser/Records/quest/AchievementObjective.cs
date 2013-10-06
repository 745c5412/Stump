 


// Generated on 10/06/2013 14:22:01
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("AchievementObjectives")]
    [D2OClass("AchievementObjective")]
    public class AchievementObjectiveRecord : ID2ORecord
    {
        private const String MODULE = "AchievementObjectives";
        public uint id;
        public uint achievementId;
        public uint nameId;
        public String criterion;

        [PrimaryKey("Id", false)]
        public uint Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint AchievementId
        {
            get { return achievementId; }
            set { achievementId = value; }
        }

        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        [NullString]
        public String Criterion
        {
            get { return criterion; }
            set { criterion = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (AchievementObjective)obj;
            
            Id = castedObj.id;
            AchievementId = castedObj.achievementId;
            NameId = castedObj.nameId;
            Criterion = castedObj.criterion;
        }
        
        public virtual object CreateObject()
        {
            
            var obj = new AchievementObjective();
            obj.id = Id;
            obj.achievementId = AchievementId;
            obj.nameId = NameId;
            obj.criterion = Criterion;
            return obj;
        
        }
    }
}