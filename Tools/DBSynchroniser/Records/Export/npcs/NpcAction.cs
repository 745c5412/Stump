// Generated on 11/02/2013 14:55:50
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;
using System;

namespace DBSynchroniser.Records
{
    [TableName("NpcActions")]
    [D2OClass("NpcAction", "com.ankamagames.dofus.datacenter.npcs")]
    public class NpcActionRecord : ID2ORecord, ISaveIntercepter
    {
        private const String MODULE = "NpcActions";
        public int id;

        [I18NField]
        public uint nameId;

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
        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (NpcAction)obj;

            Id = castedObj.id;
            NameId = castedObj.nameId;
        }

        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (NpcAction)parent : new NpcAction();
            obj.id = Id;
            obj.nameId = NameId;
            return obj;
        }

        public virtual void BeforeSave(bool insert)
        {
        }
    }
}