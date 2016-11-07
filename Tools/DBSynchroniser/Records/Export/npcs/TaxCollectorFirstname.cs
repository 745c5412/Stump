// Generated on 11/02/2013 14:55:50
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;
using System;

namespace DBSynchroniser.Records
{
    [TableName("TaxCollectorFirstnames")]
    [D2OClass("TaxCollectorFirstname", "com.ankamagames.dofus.datacenter.npcs")]
    public class TaxCollectorFirstnameRecord : ID2ORecord, ISaveIntercepter
    {
        private const String MODULE = "TaxCollectorFirstnames";
        public int id;

        [I18NField]
        public uint firstnameId;

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
        public uint FirstnameId
        {
            get { return firstnameId; }
            set { firstnameId = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (TaxCollectorFirstname)obj;

            Id = castedObj.id;
            FirstnameId = castedObj.firstnameId;
        }

        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (TaxCollectorFirstname)parent : new TaxCollectorFirstname();
            obj.id = Id;
            obj.firstnameId = FirstnameId;
            return obj;
        }

        public virtual void BeforeSave(bool insert)
        {
        }
    }
}