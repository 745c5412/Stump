
// Generated on 03/25/2013 19:24:06
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ChatSmileyMessage : Message
    {
        public const uint Id = 801;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int entityId;
        public sbyte smileyId;
        public int accountId;
        
        public ChatSmileyMessage()
        {
        }
        
        public ChatSmileyMessage(int entityId, sbyte smileyId, int accountId)
        {
            this.entityId = entityId;
            this.smileyId = smileyId;
            this.accountId = accountId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(entityId);
            writer.WriteSByte(smileyId);
            writer.WriteInt(accountId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            entityId = reader.ReadInt();
            smileyId = reader.ReadSByte();
            if (smileyId < 0)
                throw new Exception("Forbidden value on smileyId = " + smileyId + ", it doesn't respect the following condition : smileyId < 0");
            accountId = reader.ReadInt();
            if (accountId < 0)
                throw new Exception("Forbidden value on accountId = " + accountId + ", it doesn't respect the following condition : accountId < 0");
        }
        
    }
    
}