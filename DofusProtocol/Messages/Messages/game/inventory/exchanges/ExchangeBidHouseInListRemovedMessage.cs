

// Generated on 01/04/2015 11:54:28
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeBidHouseInListRemovedMessage : Message
    {
        public const uint Id = 5950;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int itemUID;
        
        public ExchangeBidHouseInListRemovedMessage()
        {
        }
        
        public ExchangeBidHouseInListRemovedMessage(int itemUID)
        {
            this.itemUID = itemUID;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(itemUID);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            itemUID = reader.ReadInt();
        }
        
    }
    
}