

// Generated on 01/04/2015 11:54:11
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameFightTurnFinishMessage : Message
    {
        public const uint Id = 718;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public GameFightTurnFinishMessage()
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