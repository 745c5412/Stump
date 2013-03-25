
// Generated on 03/25/2013 19:24:25
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PartInfoMessage : Message
    {
        public const uint Id = 1508;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.ContentPart part;
        public float installationPercent;
        
        public PartInfoMessage()
        {
        }
        
        public PartInfoMessage(Types.ContentPart part, float installationPercent)
        {
            this.part = part;
            this.installationPercent = installationPercent;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            part.Serialize(writer);
            writer.WriteFloat(installationPercent);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            part = new Types.ContentPart();
            part.Deserialize(reader);
            installationPercent = reader.ReadFloat();
        }
        
    }
    
}