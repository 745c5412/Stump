

// Generated on 12/12/2013 16:57:09
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class FriendWarnOnLevelGainStateMessage : Message
    {
        public const uint Id = 6078;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool enable;
        
        public FriendWarnOnLevelGainStateMessage()
        {
        }
        
        public FriendWarnOnLevelGainStateMessage(bool enable)
        {
            this.enable = enable;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(enable);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            enable = reader.ReadBoolean();
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(bool);
        }
        
    }
    
}