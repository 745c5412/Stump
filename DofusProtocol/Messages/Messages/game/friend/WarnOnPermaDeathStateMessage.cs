

// Generated on 09/01/2015 10:48:18
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class WarnOnPermaDeathStateMessage : Message
    {
        public const uint Id = 6513;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool enable;
        
        public WarnOnPermaDeathStateMessage()
        {
        }
        
        public WarnOnPermaDeathStateMessage(bool enable)
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
        
    }
    
}