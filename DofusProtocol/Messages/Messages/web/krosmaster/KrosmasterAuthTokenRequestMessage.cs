

// Generated on 02/17/2017 01:58:30
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class KrosmasterAuthTokenRequestMessage : Message
    {
        public const uint Id = 6346;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public KrosmasterAuthTokenRequestMessage()
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