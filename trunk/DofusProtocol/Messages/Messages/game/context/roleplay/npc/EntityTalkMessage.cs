
// Generated on 01/04/2013 14:35:50
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class EntityTalkMessage : Message
    {
        public const uint Id = 6110;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int entityId;
        public short textId;
        public IEnumerable<string> parameters;
        
        public EntityTalkMessage()
        {
        }
        
        public EntityTalkMessage(int entityId, short textId, IEnumerable<string> parameters)
        {
            this.entityId = entityId;
            this.textId = textId;
            this.parameters = parameters;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(entityId);
            writer.WriteShort(textId);
            writer.WriteUShort((ushort)parameters.Count());
            foreach (var entry in parameters)
            {
                 writer.WriteUTF(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            entityId = reader.ReadInt();
            textId = reader.ReadShort();
            if (textId < 0)
                throw new Exception("Forbidden value on textId = " + textId + ", it doesn't respect the following condition : textId < 0");
            var limit = reader.ReadUShort();
            parameters = new string[limit];
            for (int i = 0; i < limit; i++)
            {
                 (parameters as string[])[i] = reader.ReadUTF();
            }
        }
        
    }
    
}