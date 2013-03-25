
// Generated on 03/25/2013 19:24:18
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class TaxCollectorMovementMessage : Message
    {
        public const uint Id = 5633;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool hireOrFire;
        public Types.TaxCollectorBasicInformations basicInfos;
        public string playerName;
        
        public TaxCollectorMovementMessage()
        {
        }
        
        public TaxCollectorMovementMessage(bool hireOrFire, Types.TaxCollectorBasicInformations basicInfos, string playerName)
        {
            this.hireOrFire = hireOrFire;
            this.basicInfos = basicInfos;
            this.playerName = playerName;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(hireOrFire);
            basicInfos.Serialize(writer);
            writer.WriteUTF(playerName);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            hireOrFire = reader.ReadBoolean();
            basicInfos = new Types.TaxCollectorBasicInformations();
            basicInfos.Deserialize(reader);
            playerName = reader.ReadUTF();
        }
        
    }
    
}