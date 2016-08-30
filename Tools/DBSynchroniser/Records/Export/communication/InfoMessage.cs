// Generated on 11/02/2013 14:55:47
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;
using System;

namespace DBSynchroniser.Records
{
    [TableName("InfoMessages")]
    [D2OClass("InfoMessage", "com.ankamagames.dofus.datacenter.communication")]
    public class InfoMessageRecord : ID2ORecord, ISaveIntercepter
    {
        private const String MODULE = "InfoMessages";
        public uint typeId;
        public uint messageId;

        [I18NField]
        public uint textId;

        int ID2ORecord.Id
        {
            get { return (int)(typeId * 10000 + messageId); }
        }

        [D2OIgnore]
        [PrimaryKey("Id")]
        public int Id
        {
            get { return (int)(typeId * 10000 + messageId); }
            set { }
        }

        [D2OIgnore]
        public uint TypeId
        {
            get { return typeId; }
            set { typeId = value; }
        }

        [D2OIgnore]
        public uint MessageId
        {
            get { return messageId; }
            set { messageId = value; }
        }

        [D2OIgnore]
        [I18NField]
        public uint TextId
        {
            get { return textId; }
            set { textId = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (InfoMessage)obj;

            TypeId = castedObj.typeId;
            MessageId = castedObj.messageId;
            TextId = castedObj.textId;
        }

        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (InfoMessage)parent : new InfoMessage();
            obj.typeId = TypeId;
            obj.messageId = MessageId;
            obj.textId = TextId;
            return obj;
        }

        public virtual void BeforeSave(bool insert)
        {
        }
    }
}