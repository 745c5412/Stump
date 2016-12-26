

// Generated on 12/26/2016 21:57:52
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class DareVersatileListMessage : Message
    {
        public const uint Id = 6657;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.DareVersatileInformations> dares;
        
        public DareVersatileListMessage()
        {
        }
        
        public DareVersatileListMessage(IEnumerable<Types.DareVersatileInformations> dares)
        {
            this.dares = dares;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            var dares_before = writer.Position;
            var dares_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in dares)
            {
                 entry.Serialize(writer);
                 dares_count++;
            }
            var dares_after = writer.Position;
            writer.Seek((int)dares_before);
            writer.WriteUShort((ushort)dares_count);
            writer.Seek((int)dares_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            var dares_ = new Types.DareVersatileInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 dares_[i] = new Types.DareVersatileInformations();
                 dares_[i].Deserialize(reader);
            }
            dares = dares_;
        }
        
    }
    
}