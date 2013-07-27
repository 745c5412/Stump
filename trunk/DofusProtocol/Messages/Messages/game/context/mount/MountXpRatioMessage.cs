

// Generated on 07/26/2013 22:50:55
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class MountXpRatioMessage : Message
    {
        public const uint Id = 5970;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte ratio;
        
        public MountXpRatioMessage()
        {
        }
        
        public MountXpRatioMessage(sbyte ratio)
        {
            this.ratio = ratio;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(ratio);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            ratio = reader.ReadSByte();
            if (ratio < 0)
                throw new Exception("Forbidden value on ratio = " + ratio + ", it doesn't respect the following condition : ratio < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(sbyte);
        }
        
    }
    
}