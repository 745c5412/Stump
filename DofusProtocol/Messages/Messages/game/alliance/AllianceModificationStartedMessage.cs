

// Generated on 12/26/2016 21:57:35
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class AllianceModificationStartedMessage : Message
    {
        public const uint Id = 6444;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool canChangeName;
        public bool canChangeTag;
        public bool canChangeEmblem;
        
        public AllianceModificationStartedMessage()
        {
        }
        
        public AllianceModificationStartedMessage(bool canChangeName, bool canChangeTag, bool canChangeEmblem)
        {
            this.canChangeName = canChangeName;
            this.canChangeTag = canChangeTag;
            this.canChangeEmblem = canChangeEmblem;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            byte flag1 = 0;
            flag1 = BooleanByteWrapper.SetFlag(flag1, 0, canChangeName);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 1, canChangeTag);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 2, canChangeEmblem);
            writer.WriteByte(flag1);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            byte flag1 = reader.ReadByte();
            canChangeName = BooleanByteWrapper.GetFlag(flag1, 0);
            canChangeTag = BooleanByteWrapper.GetFlag(flag1, 1);
            canChangeEmblem = BooleanByteWrapper.GetFlag(flag1, 2);
        }
        
    }
    
}