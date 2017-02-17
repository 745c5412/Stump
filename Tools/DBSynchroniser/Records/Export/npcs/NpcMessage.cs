 


// Generated on 02/14/2017 17:01:38
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("NpcMessages")]
    [D2OClass("NpcMessage", "com.ankamagames.dofus.datacenter.npcs")]
    public class NpcMessageRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "NpcMessages";
        public int id;
        [I18NField]
        public uint messageId;
        public List<String> messageParams;

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
        public uint MessageId
        {
            get { return messageId; }
            set { messageId = value; }
        }

        [D2OIgnore]
        [Ignore]
        public List<String> MessageParams
        {
            get { return messageParams; }
            set
            {
                messageParams = value;
                m_messageParamsBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_messageParamsBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] MessageParamsBin
        {
            get { return m_messageParamsBin; }
            set
            {
                m_messageParamsBin = value;
                messageParams = value == null ? null : value.ToObject<List<String>>();
            }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (NpcMessage)obj;
            
            Id = castedObj.id;
            MessageId = castedObj.messageId;
            MessageParams = castedObj.messageParams;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (NpcMessage)parent : new NpcMessage();
            obj.id = Id;
            obj.messageId = MessageId;
            obj.messageParams = MessageParams;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
            m_messageParamsBin = messageParams == null ? null : messageParams.ToBinary();
        
        }
    }
}