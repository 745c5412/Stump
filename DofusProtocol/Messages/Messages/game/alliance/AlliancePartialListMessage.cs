

// Generated on 12/20/2015 16:36:44
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class AlliancePartialListMessage : AllianceListMessage
    {
        public const uint Id = 6427;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public AlliancePartialListMessage()
        {
        }
        
        public AlliancePartialListMessage(IEnumerable<Types.AllianceFactSheetInformations> alliances)
         : base(alliances)
        {
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
        }
        
    }
    
}