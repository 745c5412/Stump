

// Generated on 01/04/2015 11:54:41
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ClientKeyMessage : Message
    {
        public const uint Id = 5607;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string key;
        
        public ClientKeyMessage()
        {
        }
        
        public ClientKeyMessage(string key)
        {
            this.key = key;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUTF(key);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            key = reader.ReadUTF();
        }
        
    }
    
}