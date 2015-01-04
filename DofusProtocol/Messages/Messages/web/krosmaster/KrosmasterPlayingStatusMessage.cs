

// Generated on 01/04/2015 11:54:42
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class KrosmasterPlayingStatusMessage : Message
    {
        public const uint Id = 6347;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool playing;
        
        public KrosmasterPlayingStatusMessage()
        {
        }
        
        public KrosmasterPlayingStatusMessage(bool playing)
        {
            this.playing = playing;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(playing);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            playing = reader.ReadBoolean();
        }
        
    }
    
}