
// Generated on 03/25/2013 19:24:12
using System;
using System.Collections.Generic;
using System.Linq;
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
        
    }
    
}