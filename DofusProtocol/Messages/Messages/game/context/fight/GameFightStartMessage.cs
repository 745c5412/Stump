

// Generated on 02/18/2015 10:46:11
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameFightStartMessage : Message
    {
        public const uint Id = 712;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public GameFightStartMessage()
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