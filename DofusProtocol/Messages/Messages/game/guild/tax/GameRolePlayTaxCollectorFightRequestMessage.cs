

// Generated on 12/26/2016 21:57:55
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameRolePlayTaxCollectorFightRequestMessage : Message
    {
        public const uint Id = 5954;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int taxCollectorId;
        
        public GameRolePlayTaxCollectorFightRequestMessage()
        {
        }
        
        public GameRolePlayTaxCollectorFightRequestMessage(int taxCollectorId)
        {
            this.taxCollectorId = taxCollectorId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(taxCollectorId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            taxCollectorId = reader.ReadInt();
        }
        
    }
    
}