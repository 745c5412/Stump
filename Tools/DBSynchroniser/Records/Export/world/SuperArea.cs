// Generated on 11/02/2013 14:55:51
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;
using System;

namespace DBSynchroniser.Records
{
    [TableName("SuperAreas")]
    [D2OClass("SuperArea", "com.ankamagames.dofus.datacenter.world")]
    public class SuperAreaRecord : ID2ORecord, ISaveIntercepter
    {
        private const String MODULE = "SuperAreas";
        public int id;

        [I18NField]
        public uint nameId;

        public uint worldmapId;

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

        [D2OIgnore]
        public uint WorldmapId
        {
            get { return worldmapId; }
            set { worldmapId = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (SuperArea)obj;

            Id = castedObj.id;
            NameId = castedObj.nameId;
            WorldmapId = castedObj.worldmapId;
        }

        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (SuperArea)parent : new SuperArea();
            obj.id = Id;
            obj.nameId = NameId;
            obj.worldmapId = WorldmapId;
            return obj;
        }

        public virtual void BeforeSave(bool insert)
        {
        }
    }
}