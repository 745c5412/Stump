

// Generated on 07/26/2013 22:51:07
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PrismBalanceResultMessage : Message
    {
        public const uint Id = 5841;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte totalBalanceValue;
        public sbyte subAreaBalanceValue;
        
        public PrismBalanceResultMessage()
        {
        }
        
        public PrismBalanceResultMessage(sbyte totalBalanceValue, sbyte subAreaBalanceValue)
        {
            this.totalBalanceValue = totalBalanceValue;
            this.subAreaBalanceValue = subAreaBalanceValue;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(totalBalanceValue);
            writer.WriteSByte(subAreaBalanceValue);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            totalBalanceValue = reader.ReadSByte();
            if (totalBalanceValue < 0)
                throw new Exception("Forbidden value on totalBalanceValue = " + totalBalanceValue + ", it doesn't respect the following condition : totalBalanceValue < 0");
            subAreaBalanceValue = reader.ReadSByte();
            if (subAreaBalanceValue < 0)
                throw new Exception("Forbidden value on subAreaBalanceValue = " + subAreaBalanceValue + ", it doesn't respect the following condition : subAreaBalanceValue < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(sbyte) + sizeof(sbyte);
        }
        
    }
    
}