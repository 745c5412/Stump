

// Generated on 10/26/2014 23:29:39
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeWaitingResultMessage : Message
    {
        public const uint Id = 5786;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool bwait;
        
        public ExchangeWaitingResultMessage()
        {
        }
        
        public ExchangeWaitingResultMessage(bool bwait)
        {
            this.bwait = bwait;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(bwait);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            bwait = reader.ReadBoolean();
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(bool);
        }
        
    }
    
}