

// Generated on 12/26/2016 21:57:48
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PartyInvitationRequestMessage : Message
    {
        public const uint Id = 5585;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string name;
        
        public PartyInvitationRequestMessage()
        {
        }
        
        public PartyInvitationRequestMessage(string name)
        {
            this.name = name;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUTF(name);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            name = reader.ReadUTF();
        }
        
    }
    
}