// Generated on 11/02/2013 14:55:49
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;
using System;

namespace DBSynchroniser.Records
{
    [TableName("TypeActions")]
    [D2OClass("TypeAction", "com.ankamagames.dofus.datacenter.misc")]
    public class TypeActionRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "TypeActions";
        public int id;
        public String elementName;
        public int elementId;

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
        [NullString]
        public String ElementName
        {
            get { return elementName; }
            set { elementName = value; }
        }

        [D2OIgnore]
        public int ElementId
        {
            get { return elementId; }
            set { elementId = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (TypeAction)obj;

            Id = castedObj.id;
            ElementName = castedObj.elementName;
            ElementId = castedObj.elementId;
        }

        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (TypeAction)parent : new TypeAction();
            obj.id = Id;
            obj.elementName = ElementName;
            obj.elementId = ElementId;
            return obj;
        }

        public virtual void BeforeSave(bool insert)
        {
        }
    }
}