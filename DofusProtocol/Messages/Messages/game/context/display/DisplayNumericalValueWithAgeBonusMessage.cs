

// Generated on 03/02/2014 20:42:36
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class DisplayNumericalValueWithAgeBonusMessage : DisplayNumericalValueMessage
    {
        public const uint Id = 6361;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int valueOfBonus;
        
        public DisplayNumericalValueWithAgeBonusMessage()
        {
        }
        
        public DisplayNumericalValueWithAgeBonusMessage(int entityId, int value, sbyte type, int valueOfBonus)
         : base(entityId, value, type)
        {
            this.valueOfBonus = valueOfBonus;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(valueOfBonus);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            valueOfBonus = reader.ReadInt();
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(int);
        }
        
    }
    
}