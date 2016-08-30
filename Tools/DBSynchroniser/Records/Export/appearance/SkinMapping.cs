// Generated on 11/02/2013 14:55:46
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;
using System;

namespace DBSynchroniser.Records
{
    [TableName("SkinMappings")]
    [D2OClass("SkinMapping", "com.ankamagames.dofus.datacenter.appearance")]
    public class SkinMappingRecord : ID2ORecord, ISaveIntercepter
    {
        private const String MODULE = "SkinMappings";
        public int id;
        public int lowDefId;

        int ID2ORecord.Id
        {
            get { return (int)id; }
        }

        [D2OIgnore]
        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [D2OIgnore]
        public int LowDefId
        {
            get { return lowDefId; }
            set { lowDefId = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (SkinMapping)obj;

            Id = castedObj.id;
            LowDefId = castedObj.lowDefId;
        }

        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (SkinMapping)parent : new SkinMapping();
            obj.id = Id;
            obj.lowDefId = LowDefId;
            return obj;
        }

        public virtual void BeforeSave(bool insert)
        {
        }
    }
}