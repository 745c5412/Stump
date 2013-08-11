

// Generated on 08/11/2013 11:29:01
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ObjectsQuantityMessage : Message
    {
        public const uint Id = 6206;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.ObjectItemQuantity> objectsUIDAndQty;
        
        public ObjectsQuantityMessage()
        {
        }
        
        public ObjectsQuantityMessage(IEnumerable<Types.ObjectItemQuantity> objectsUIDAndQty)
        {
            this.objectsUIDAndQty = objectsUIDAndQty;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)objectsUIDAndQty.Count());
            foreach (var entry in objectsUIDAndQty)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            objectsUIDAndQty = new Types.ObjectItemQuantity[limit];
            for (int i = 0; i < limit; i++)
            {
                 (objectsUIDAndQty as Types.ObjectItemQuantity[])[i] = new Types.ObjectItemQuantity();
                 (objectsUIDAndQty as Types.ObjectItemQuantity[])[i].Deserialize(reader);
            }
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + objectsUIDAndQty.Sum(x => x.GetSerializationSize());
        }
        
    }
    
}