
// Generated on 03/25/2013 19:23:58
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class AcquaintanceServerListMessage : Message
    {
        public const uint Id = 6142;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<short> servers;
        
        public AcquaintanceServerListMessage()
        {
        }
        
        public AcquaintanceServerListMessage(IEnumerable<short> servers)
        {
            this.servers = servers;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)servers.Count());
            foreach (var entry in servers)
            {
                 writer.WriteShort(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            servers = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 (servers as short[])[i] = reader.ReadShort();
            }
        }
        
    }
    
}