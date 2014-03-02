

// Generated on 03/02/2014 20:42:54
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ObjectMovementMessage : Message
    {
        public const uint Id = 3010;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int objectUID;
        public byte position;
        
        public ObjectMovementMessage()
        {
        }
        
        public ObjectMovementMessage(int objectUID, byte position)
        {
            this.objectUID = objectUID;
            this.position = position;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(objectUID);
            writer.WriteByte(position);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            objectUID = reader.ReadInt();
            if (objectUID < 0)
                throw new Exception("Forbidden value on objectUID = " + objectUID + ", it doesn't respect the following condition : objectUID < 0");
            position = reader.ReadByte();
            if (position < 0 || position > 255)
                throw new Exception("Forbidden value on position = " + position + ", it doesn't respect the following condition : position < 0 || position > 255");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(int) + sizeof(byte);
        }
        
    }
    
}