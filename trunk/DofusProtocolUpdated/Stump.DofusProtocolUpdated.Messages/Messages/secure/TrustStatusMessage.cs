

// Generated on 12/12/2013 16:57:25
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class TrustStatusMessage : Message
    {
        public const uint Id = 6267;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool trusted;
        
        public TrustStatusMessage()
        {
        }
        
        public TrustStatusMessage(bool trusted)
        {
            this.trusted = trusted;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(trusted);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            trusted = reader.ReadBoolean();
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(bool);
        }
        
    }
    
}