

// Generated on 02/18/2015 10:46:10
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameMapNoMovementMessage : Message
    {
        public const uint Id = 954;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public GameMapNoMovementMessage()
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