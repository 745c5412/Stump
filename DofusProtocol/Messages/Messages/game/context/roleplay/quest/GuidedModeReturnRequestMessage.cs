

// Generated on 12/26/2016 21:57:50
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GuidedModeReturnRequestMessage : Message
    {
        public const uint Id = 6088;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public GuidedModeReturnRequestMessage()
        {
        }
        
        
        public override void Serialize(IDataWriter writer)
        {
        }
        
        public override void Deserialize(IDataReader reader)
        {
        }
        
    }
    
}