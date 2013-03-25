
// Generated on 03/25/2013 19:24:14
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PartyModifiableStatusMessage : AbstractPartyMessage
    {
        public const uint Id = 6277;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool enabled;
        
        public PartyModifiableStatusMessage()
        {
        }
        
        public PartyModifiableStatusMessage(int partyId, bool enabled)
         : base(partyId)
        {
            this.enabled = enabled;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteBoolean(enabled);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            enabled = reader.ReadBoolean();
        }
        
    }
    
}