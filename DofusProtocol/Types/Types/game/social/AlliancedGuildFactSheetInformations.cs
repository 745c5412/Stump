

// Generated on 12/26/2016 21:58:16
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class AlliancedGuildFactSheetInformations : GuildInformations
    {
        public const short Id = 422;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public Types.BasicNamedAllianceInformations allianceInfos;
        
        public AlliancedGuildFactSheetInformations()
        {
        }
        
        public AlliancedGuildFactSheetInformations(int guildId, string guildName, byte guildLevel, Types.GuildEmblem guildEmblem, Types.BasicNamedAllianceInformations allianceInfos)
         : base(guildId, guildName, guildLevel, guildEmblem)
        {
            this.allianceInfos = allianceInfos;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            allianceInfos.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            allianceInfos = new Types.BasicNamedAllianceInformations();
            allianceInfos.Deserialize(reader);
        }
        
        
    }
    
}