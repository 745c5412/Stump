

// Generated on 12/26/2016 21:57:39
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameEntityDispositionErrorMessage : Message
    {
        public const uint Id = 5695;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public GameEntityDispositionErrorMessage()
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