

// Generated on 03/02/2014 20:42:42
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class NpcDialogReplyMessage : Message
    {
        public const uint Id = 5616;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short replyId;
        
        public NpcDialogReplyMessage()
        {
        }
        
        public NpcDialogReplyMessage(short replyId)
        {
            this.replyId = replyId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(replyId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            replyId = reader.ReadShort();
            if (replyId < 0)
                throw new Exception("Forbidden value on replyId = " + replyId + ", it doesn't respect the following condition : replyId < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short);
        }
        
    }
    
}