

// Generated on 12/26/2016 21:57:37
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class CharacterLevelUpMessage : Message
    {
        public const uint Id = 5670;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public byte newLevel;
        
        public CharacterLevelUpMessage()
        {
        }
        
        public CharacterLevelUpMessage(byte newLevel)
        {
            this.newLevel = newLevel;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteByte(newLevel);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            newLevel = reader.ReadByte();
            if (newLevel < 2 || newLevel > 206)
                throw new Exception("Forbidden value on newLevel = " + newLevel + ", it doesn't respect the following condition : newLevel < 2 || newLevel > 206");
        }
        
    }
    
}