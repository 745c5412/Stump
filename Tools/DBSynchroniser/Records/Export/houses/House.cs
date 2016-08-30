// Generated on 11/02/2013 14:55:47
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;
using System;

namespace DBSynchroniser.Records
{
    [TableName("Houses")]
    [D2OClass("House", "com.ankamagames.dofus.datacenter.houses")]
    public class HouseRecord : ID2ORecord, ISaveIntercepter
    {
        private const String MODULE = "Houses";
        public int typeId;
        public uint defaultPrice;

        [I18NField]
        public int nameId;

        [I18NField]
        public int descriptionId;

        public int gfxId;

        int ID2ORecord.Id
        {
            get { return (int)nameId; }
        }

        [D2OIgnore]
        public int TypeId
        {
            get { return typeId; }
            set { typeId = value; }
        }

        [D2OIgnore]
        public uint DefaultPrice
        {
            get { return defaultPrice; }
            set { defaultPrice = value; }
        }

        [D2OIgnore]
        [PrimaryKey("NameId", false)]
        [I18NField]
        public int NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        [D2OIgnore]
        [I18NField]
        public int DescriptionId
        {
            get { return descriptionId; }
            set { descriptionId = value; }
        }

        [D2OIgnore]
        public int GfxId
        {
            get { return gfxId; }
            set { gfxId = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (House)obj;

            TypeId = castedObj.typeId;
            DefaultPrice = castedObj.defaultPrice;
            NameId = castedObj.nameId;
            DescriptionId = castedObj.descriptionId;
            GfxId = castedObj.gfxId;
        }

        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (House)parent : new House();
            obj.typeId = TypeId;
            obj.defaultPrice = DefaultPrice;
            obj.nameId = NameId;
            obj.descriptionId = DescriptionId;
            obj.gfxId = GfxId;
            return obj;
        }

        public virtual void BeforeSave(bool insert)
        {
        }
    }
}