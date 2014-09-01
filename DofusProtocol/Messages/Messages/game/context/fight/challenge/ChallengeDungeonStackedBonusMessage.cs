

// Generated on 09/01/2014 15:51:57
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ChallengeDungeonStackedBonusMessage : Message
    {
        public const uint Id = 6151;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int dungeonId;
        public int xpBonus;
        public int dropBonus;
        
        public ChallengeDungeonStackedBonusMessage()
        {
        }
        
        public ChallengeDungeonStackedBonusMessage(int dungeonId, int xpBonus, int dropBonus)
        {
            this.dungeonId = dungeonId;
            this.xpBonus = xpBonus;
            this.dropBonus = dropBonus;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(dungeonId);
            writer.WriteInt(xpBonus);
            writer.WriteInt(dropBonus);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            dungeonId = reader.ReadInt();
            if (dungeonId < 0)
                throw new Exception("Forbidden value on dungeonId = " + dungeonId + ", it doesn't respect the following condition : dungeonId < 0");
            xpBonus = reader.ReadInt();
            if (xpBonus < 0)
                throw new Exception("Forbidden value on xpBonus = " + xpBonus + ", it doesn't respect the following condition : xpBonus < 0");
            dropBonus = reader.ReadInt();
            if (dropBonus < 0)
                throw new Exception("Forbidden value on dropBonus = " + dropBonus + ", it doesn't respect the following condition : dropBonus < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(int) + sizeof(int) + sizeof(int);
        }
        
    }
    
}