

// Generated on 09/01/2014 15:51:55
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameContextRefreshEntityLookMessage : Message
    {
        public const uint Id = 5637;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int id;
        public Types.EntityLook look;
        
        public GameContextRefreshEntityLookMessage()
        {
        }
        
        public GameContextRefreshEntityLookMessage(int id, Types.EntityLook look)
        {
            this.id = id;
            this.look = look;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(id);
            look.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            id = reader.ReadInt();
            look = new Types.EntityLook();
            look.Deserialize(reader);
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(int) + look.GetSerializationSize();
        }
        
    }
    
}