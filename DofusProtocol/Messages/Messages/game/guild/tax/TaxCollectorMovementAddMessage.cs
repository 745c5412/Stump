

// Generated on 02/18/2015 10:46:22
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class TaxCollectorMovementAddMessage : Message
    {
        public const uint Id = 5917;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.TaxCollectorInformations informations;
        
        public TaxCollectorMovementAddMessage()
        {
        }
        
        public TaxCollectorMovementAddMessage(Types.TaxCollectorInformations informations)
        {
            this.informations = informations;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(informations.TypeId);
            informations.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            informations = Types.ProtocolTypeManager.GetInstance<Types.TaxCollectorInformations>(reader.ReadShort());
            informations.Deserialize(reader);
        }
        
    }
    
}