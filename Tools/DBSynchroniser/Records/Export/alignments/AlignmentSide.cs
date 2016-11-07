// Generated on 11/02/2013 14:55:46
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;
using System;

namespace DBSynchroniser.Records
{
    [TableName("AlignmentSides")]
    [D2OClass("AlignmentSide", "com.ankamagames.dofus.datacenter.alignments")]
    public class AlignmentSideRecord : ID2ORecord, ISaveIntercepter
    {
        private const String MODULE = "AlignmentSides";
        public int id;

        [I18NField]
        public uint nameId;

        public Boolean canConquest;

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
        public Boolean CanConquest
        {
            get { return canConquest; }
            set { canConquest = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (AlignmentSide)obj;

            Id = castedObj.id;
            NameId = castedObj.nameId;
            CanConquest = castedObj.canConquest;
        }

        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (AlignmentSide)parent : new AlignmentSide();
            obj.id = Id;
            obj.nameId = NameId;
            obj.canConquest = CanConquest;
            return obj;
        }

        public virtual void BeforeSave(bool insert)
        {
        }
    }
}