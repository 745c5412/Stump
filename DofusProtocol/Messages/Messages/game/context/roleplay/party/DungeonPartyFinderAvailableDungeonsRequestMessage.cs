

// Generated on 10/27/2014 19:57:46
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class DungeonPartyFinderAvailableDungeonsRequestMessage : Message
    {
        public const uint Id = 6240;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public DungeonPartyFinderAvailableDungeonsRequestMessage()
        {
        }
        
        
        public override void Serialize(IDataWriter writer)
        {
        }
        
        public override void Deserialize(IDataReader reader)
        {
        }
        
        public override int GetSerializationSize()
        {
            return 0;
            ;
        }
        
    }
    
}