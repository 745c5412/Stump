

// Generated on 08/04/2015 13:24:58
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class NotificationResetMessage : Message
    {
        public const uint Id = 6089;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public NotificationResetMessage()
        {
        }
        
        
        public override void Serialize(IDataWriter writer)
        {
        }
        
        public override void Deserialize(IDataReader reader)
        {
        }
        
    }
    
}