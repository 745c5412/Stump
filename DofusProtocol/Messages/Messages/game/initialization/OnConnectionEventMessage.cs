

// Generated on 01/04/2015 11:54:26
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class OnConnectionEventMessage : Message
    {
        public const uint Id = 5726;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte eventType;
        
        public OnConnectionEventMessage()
        {
        }
        
        public OnConnectionEventMessage(sbyte eventType)
        {
            this.eventType = eventType;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(eventType);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            eventType = reader.ReadSByte();
            if (eventType < 0)
                throw new Exception("Forbidden value on eventType = " + eventType + ", it doesn't respect the following condition : eventType < 0");
        }
        
    }
    
}