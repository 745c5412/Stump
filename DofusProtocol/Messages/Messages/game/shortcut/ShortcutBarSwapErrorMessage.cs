

// Generated on 12/26/2016 21:58:06
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ShortcutBarSwapErrorMessage : Message
    {
        public const uint Id = 6226;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte error;
        
        public ShortcutBarSwapErrorMessage()
        {
        }
        
        public ShortcutBarSwapErrorMessage(sbyte error)
        {
            this.error = error;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(error);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            error = reader.ReadSByte();
            if (error < 0)
                throw new Exception("Forbidden value on error = " + error + ", it doesn't respect the following condition : error < 0");
        }
        
    }
    
}