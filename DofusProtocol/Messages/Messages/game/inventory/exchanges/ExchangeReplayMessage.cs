

// Generated on 08/04/2015 13:25:14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeReplayMessage : Message
    {
        public const uint Id = 6002;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int count;
        
        public ExchangeReplayMessage()
        {
        }
        
        public ExchangeReplayMessage(int count)
        {
            this.count = count;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarInt(count);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            count = reader.ReadVarInt();
        }
        
    }
    
}