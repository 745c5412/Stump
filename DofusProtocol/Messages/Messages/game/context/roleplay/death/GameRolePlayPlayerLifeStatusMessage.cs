

// Generated on 10/30/2016 16:20:29
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameRolePlayPlayerLifeStatusMessage : Message
    {
        public const uint Id = 5996;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte state;
        public int phenixMapId;
        
        public GameRolePlayPlayerLifeStatusMessage()
        {
        }
        
        public GameRolePlayPlayerLifeStatusMessage(sbyte state, int phenixMapId)
        {
            this.state = state;
            this.phenixMapId = phenixMapId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(state);
            writer.WriteInt(phenixMapId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            state = reader.ReadSByte();
            if (state < 0)
                throw new Exception("Forbidden value on state = " + state + ", it doesn't respect the following condition : state < 0");
            phenixMapId = reader.ReadInt();
            if (phenixMapId < 0)
                throw new Exception("Forbidden value on phenixMapId = " + phenixMapId + ", it doesn't respect the following condition : phenixMapId < 0");
        }
        
    }
    
}