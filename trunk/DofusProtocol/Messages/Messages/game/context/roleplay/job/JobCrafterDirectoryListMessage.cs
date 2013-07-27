

// Generated on 07/26/2013 22:50:57
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class JobCrafterDirectoryListMessage : Message
    {
        public const uint Id = 6046;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.JobCrafterDirectoryListEntry> listEntries;
        
        public JobCrafterDirectoryListMessage()
        {
        }
        
        public JobCrafterDirectoryListMessage(IEnumerable<Types.JobCrafterDirectoryListEntry> listEntries)
        {
            this.listEntries = listEntries;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)listEntries.Count());
            foreach (var entry in listEntries)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            listEntries = new Types.JobCrafterDirectoryListEntry[limit];
            for (int i = 0; i < limit; i++)
            {
                 (listEntries as Types.JobCrafterDirectoryListEntry[])[i] = new Types.JobCrafterDirectoryListEntry();
                 (listEntries as Types.JobCrafterDirectoryListEntry[])[i].Deserialize(reader);
            }
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + listEntries.Sum(x => x.GetSerializationSize());
        }
        
    }
    
}