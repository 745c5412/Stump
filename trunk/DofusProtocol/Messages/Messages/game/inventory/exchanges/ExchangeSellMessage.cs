
// Generated on 01/04/2013 14:35:57
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeSellMessage : Message
    {
        public const uint Id = 5778;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int objectToSellId;
        public int quantity;
        
        public ExchangeSellMessage()
        {
        }
        
        public ExchangeSellMessage(int objectToSellId, int quantity)
        {
            this.objectToSellId = objectToSellId;
            this.quantity = quantity;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(objectToSellId);
            writer.WriteInt(quantity);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            objectToSellId = reader.ReadInt();
            if (objectToSellId < 0)
                throw new Exception("Forbidden value on objectToSellId = " + objectToSellId + ", it doesn't respect the following condition : objectToSellId < 0");
            quantity = reader.ReadInt();
            if (quantity < 0)
                throw new Exception("Forbidden value on quantity = " + quantity + ", it doesn't respect the following condition : quantity < 0");
        }
        
    }
    
}