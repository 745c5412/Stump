

// Generated on 12/26/2016 21:57:44
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class HavenBagDailyLoteryMessage : Message
    {
        public const uint Id = 6644;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string tokenId;
        
        public HavenBagDailyLoteryMessage()
        {
        }
        
        public HavenBagDailyLoteryMessage(string tokenId)
        {
            this.tokenId = tokenId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUTF(tokenId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            tokenId = reader.ReadUTF();
        }
        
    }
    
}