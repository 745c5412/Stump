

// Generated on 12/26/2016 21:57:58
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeMountStableErrorMessage : Message
    {
        public const uint Id = 5981;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public ExchangeMountStableErrorMessage()
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