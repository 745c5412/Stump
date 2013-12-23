

// Generated on 12/12/2013 16:57:18
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeStartOkMulticraftCustomerMessage : Message
    {
        public const uint Id = 5817;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte maxCase;
        public int skillId;
        public sbyte crafterJobLevel;
        
        public ExchangeStartOkMulticraftCustomerMessage()
        {
        }
        
        public ExchangeStartOkMulticraftCustomerMessage(sbyte maxCase, int skillId, sbyte crafterJobLevel)
        {
            this.maxCase = maxCase;
            this.skillId = skillId;
            this.crafterJobLevel = crafterJobLevel;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(maxCase);
            writer.WriteInt(skillId);
            writer.WriteSByte(crafterJobLevel);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            maxCase = reader.ReadSByte();
            if (maxCase < 0)
                throw new Exception("Forbidden value on maxCase = " + maxCase + ", it doesn't respect the following condition : maxCase < 0");
            skillId = reader.ReadInt();
            if (skillId < 0)
                throw new Exception("Forbidden value on skillId = " + skillId + ", it doesn't respect the following condition : skillId < 0");
            crafterJobLevel = reader.ReadSByte();
            if (crafterJobLevel < 0)
                throw new Exception("Forbidden value on crafterJobLevel = " + crafterJobLevel + ", it doesn't respect the following condition : crafterJobLevel < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(sbyte) + sizeof(int) + sizeof(sbyte);
        }
        
    }
    
}