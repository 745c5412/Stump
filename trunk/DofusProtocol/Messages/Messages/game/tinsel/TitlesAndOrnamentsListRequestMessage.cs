
// Generated on 03/25/2013 19:24:25
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class TitlesAndOrnamentsListRequestMessage : Message
    {
        public const uint Id = 6363;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public TitlesAndOrnamentsListRequestMessage()
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