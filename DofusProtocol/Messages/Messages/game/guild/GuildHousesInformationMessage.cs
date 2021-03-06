

// Generated on 10/30/2016 16:20:38
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GuildHousesInformationMessage : Message
    {
        public const uint Id = 5919;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.HouseInformationsForGuild> housesInformations;
        
        public GuildHousesInformationMessage()
        {
        }
        
        public GuildHousesInformationMessage(IEnumerable<Types.HouseInformationsForGuild> housesInformations)
        {
            this.housesInformations = housesInformations;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            var housesInformations_before = writer.Position;
            var housesInformations_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in housesInformations)
            {
                 entry.Serialize(writer);
                 housesInformations_count++;
            }
            var housesInformations_after = writer.Position;
            writer.Seek((int)housesInformations_before);
            writer.WriteUShort((ushort)housesInformations_count);
            writer.Seek((int)housesInformations_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            var housesInformations_ = new Types.HouseInformationsForGuild[limit];
            for (int i = 0; i < limit; i++)
            {
                 housesInformations_[i] = new Types.HouseInformationsForGuild();
                 housesInformations_[i].Deserialize(reader);
            }
            housesInformations = housesInformations_;
        }
        
    }
    
}