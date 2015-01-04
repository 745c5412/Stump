

// Generated on 01/04/2015 11:54:21
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PartyUpdateLightMessage : AbstractPartyEventMessage
    {
        public const uint Id = 6054;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int id;
        public int lifePoints;
        public int maxLifePoints;
        public short prospecting;
        public byte regenRate;
        
        public PartyUpdateLightMessage()
        {
        }
        
        public PartyUpdateLightMessage(int partyId, int id, int lifePoints, int maxLifePoints, short prospecting, byte regenRate)
         : base(partyId)
        {
            this.id = id;
            this.lifePoints = lifePoints;
            this.maxLifePoints = maxLifePoints;
            this.prospecting = prospecting;
            this.regenRate = regenRate;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteVarInt(id);
            writer.WriteVarInt(lifePoints);
            writer.WriteVarInt(maxLifePoints);
            writer.WriteVarShort(prospecting);
            writer.WriteByte(regenRate);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            id = reader.ReadVarInt();
            if (id < 0)
                throw new Exception("Forbidden value on id = " + id + ", it doesn't respect the following condition : id < 0");
            lifePoints = reader.ReadVarInt();
            if (lifePoints < 0)
                throw new Exception("Forbidden value on lifePoints = " + lifePoints + ", it doesn't respect the following condition : lifePoints < 0");
            maxLifePoints = reader.ReadVarInt();
            if (maxLifePoints < 0)
                throw new Exception("Forbidden value on maxLifePoints = " + maxLifePoints + ", it doesn't respect the following condition : maxLifePoints < 0");
            prospecting = reader.ReadVarShort();
            if (prospecting < 0)
                throw new Exception("Forbidden value on prospecting = " + prospecting + ", it doesn't respect the following condition : prospecting < 0");
            regenRate = reader.ReadByte();
            if (regenRate < 0 || regenRate > 255)
                throw new Exception("Forbidden value on regenRate = " + regenRate + ", it doesn't respect the following condition : regenRate < 0 || regenRate > 255");
        }
        
    }
    
}