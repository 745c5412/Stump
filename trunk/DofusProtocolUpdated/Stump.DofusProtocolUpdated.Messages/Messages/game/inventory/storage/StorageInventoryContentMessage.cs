

// Generated on 12/12/2013 16:57:21
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class StorageInventoryContentMessage : InventoryContentMessage
    {
        public const uint Id = 5646;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public StorageInventoryContentMessage()
        {
        }
        
        public StorageInventoryContentMessage(IEnumerable<Types.ObjectItem> objects, int kamas)
         : base(objects, kamas)
        {
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize();
        }
        
    }
    
}