

// Generated on 12/26/2016 21:57:44
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class EnterHavenBagRequestMessage : Message
    {
        public const uint Id = 6636;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public EnterHavenBagRequestMessage()
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