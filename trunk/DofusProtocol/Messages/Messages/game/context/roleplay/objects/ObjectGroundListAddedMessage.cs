
// Generated on 03/25/2013 19:24:12
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ObjectGroundListAddedMessage : Message
    {
        public const uint Id = 5925;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<short> cells;
        public IEnumerable<int> referenceIds;
        
        public ObjectGroundListAddedMessage()
        {
        }
        
        public ObjectGroundListAddedMessage(IEnumerable<short> cells, IEnumerable<int> referenceIds)
        {
            this.cells = cells;
            this.referenceIds = referenceIds;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)cells.Count());
            foreach (var entry in cells)
            {
                 writer.WriteShort(entry);
            }
            writer.WriteUShort((ushort)referenceIds.Count());
            foreach (var entry in referenceIds)
            {
                 writer.WriteInt(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            cells = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 (cells as short[])[i] = reader.ReadShort();
            }
            limit = reader.ReadUShort();
            referenceIds = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 (referenceIds as int[])[i] = reader.ReadInt();
            }
        }
        
    }
    
}