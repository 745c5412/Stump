

// Generated on 01/04/2015 11:54:19
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class DungeonPartyFinderRegisterErrorMessage : Message
    {
        public const uint Id = 6243;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public DungeonPartyFinderRegisterErrorMessage()
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