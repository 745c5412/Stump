

// Generated on 12/26/2016 21:58:13
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class GuildInformations : BasicGuildInformations
    {
        public const short Id = 127;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public Types.GuildEmblem guildEmblem;
        
        public GuildInformations()
        {
        }
        
        public GuildInformations(int guildId, string guildName, byte guildLevel, Types.GuildEmblem guildEmblem)
         : base(guildId, guildName, guildLevel)
        {
            this.guildEmblem = guildEmblem;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            guildEmblem.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            guildEmblem = new Types.GuildEmblem();
            guildEmblem.Deserialize(reader);
        }
        
        
    }
    
}