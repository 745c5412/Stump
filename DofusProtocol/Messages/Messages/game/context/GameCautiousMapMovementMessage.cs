

// Generated on 12/26/2016 21:57:38
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameCautiousMapMovementMessage : GameMapMovementMessage
    {
        public const uint Id = 6497;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public GameCautiousMapMovementMessage()
        {
        }
        
        public GameCautiousMapMovementMessage(IEnumerable<short> keyMovements, double actorId)
         : base(keyMovements, actorId)
        {
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
        }
        
    }
    
}