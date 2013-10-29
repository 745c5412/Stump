

// Generated on 10/28/2013 14:03:20
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("NpcMessage", "com.ankamagames.dofus.datacenter.npcs")]
    [Serializable]
    public class NpcMessage : IDataObject, IIndexedData
    {
        private const String MODULE = "NpcMessages";
        public int id;
        [I18NField]
        public uint messageId;
        public String messageParams;
        int IIndexedData.Id
        {
            get { return (int)id; }
        }
        [D2OIgnore]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        [D2OIgnore]
        public uint MessageId
        {
            get { return messageId; }
            set { messageId = value; }
        }
        [D2OIgnore]
        public String MessageParams
        {
            get { return messageParams; }
            set { messageParams = value; }
        }
    }
}