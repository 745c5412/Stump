

// Generated on 02/17/2017 01:57:41
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class SequenceNumberMessage : Message
    {
        public const uint Id = 6317;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short number;
        
        public SequenceNumberMessage()
        {
        }
        
        public SequenceNumberMessage(short number)
        {
            this.number = number;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(number);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            number = reader.ReadShort();
            if (number < 0 || number > 65535)
                throw new Exception("Forbidden value on number = " + number + ", it doesn't respect the following condition : number < 0 || number > 65535");
        }
        
    }
    
}