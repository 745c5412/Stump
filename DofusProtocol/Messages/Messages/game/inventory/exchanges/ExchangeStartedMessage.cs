

// Generated on 01/04/2015 11:54:33
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeStartedMessage : Message
    {
        public const uint Id = 5512;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte exchangeType;
        
        public ExchangeStartedMessage()
        {
        }
        
        public ExchangeStartedMessage(sbyte exchangeType)
        {
            this.exchangeType = exchangeType;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(exchangeType);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            exchangeType = reader.ReadSByte();
        }
        
    }
    
}