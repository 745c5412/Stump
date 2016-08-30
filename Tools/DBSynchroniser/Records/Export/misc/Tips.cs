// Generated on 11/02/2013 14:55:49
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;
using System;

namespace DBSynchroniser.Records
{
    [TableName("Tips")]
    [D2OClass("Tips", "com.ankamagames.dofus.datacenter.misc")]
    public class TipsRecord : ID2ORecord, ISaveIntercepter
    {
        private const String MODULE = "Tips";
        public int id;

        [I18NField]
        public uint descId;

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
        [I18NField]
        public uint DescId
        {
            get { return descId; }
            set { descId = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (Tips)obj;

            Id = castedObj.id;
            DescId = castedObj.descId;
        }

        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (Tips)parent : new Tips();
            obj.id = Id;
            obj.descId = DescId;
            return obj;
        }

        public virtual void BeforeSave(bool insert)
        {
        }
    }
}