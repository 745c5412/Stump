
// Generated on 03/25/2013 19:24:29
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class GameRolePlayMerchantWithGuildInformations : GameRolePlayMerchantInformations
    {
        public const short Id = 146;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public Types.GuildInformations guildInformations;
        
        public GameRolePlayMerchantWithGuildInformations()
        {
        }
        
        public GameRolePlayMerchantWithGuildInformations(int contextualId, Types.EntityLook look, Types.EntityDispositionInformations disposition, string name, int sellType, Types.GuildInformations guildInformations)
         : base(contextualId, look, disposition, name, sellType)
        {
            this.guildInformations = guildInformations;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            guildInformations.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            guildInformations = new Types.GuildInformations();
            guildInformations.Deserialize(reader);
        }
        
    }
    
}