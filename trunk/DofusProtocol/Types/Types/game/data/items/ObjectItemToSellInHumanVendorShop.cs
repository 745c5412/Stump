

// Generated on 07/26/2013 22:51:12
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class ObjectItemToSellInHumanVendorShop : Item
    {
        public const short Id = 359;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public short objectGID;
        public short powerRate;
        public bool overMax;
        public IEnumerable<Types.ObjectEffect> effects;
        public int objectUID;
        public int quantity;
        public int objectPrice;
        public int publicPrice;
        
        public ObjectItemToSellInHumanVendorShop()
        {
        }
        
        public ObjectItemToSellInHumanVendorShop(short objectGID, short powerRate, bool overMax, IEnumerable<Types.ObjectEffect> effects, int objectUID, int quantity, int objectPrice, int publicPrice)
        {
            this.objectGID = objectGID;
            this.powerRate = powerRate;
            this.overMax = overMax;
            this.effects = effects;
            this.objectUID = objectUID;
            this.quantity = quantity;
            this.objectPrice = objectPrice;
            this.publicPrice = publicPrice;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(objectGID);
            writer.WriteShort(powerRate);
            writer.WriteBoolean(overMax);
            writer.WriteUShort((ushort)effects.Count());
            foreach (var entry in effects)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
            writer.WriteInt(objectUID);
            writer.WriteInt(quantity);
            writer.WriteInt(objectPrice);
            writer.WriteInt(publicPrice);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            objectGID = reader.ReadShort();
            if (objectGID < 0)
                throw new Exception("Forbidden value on objectGID = " + objectGID + ", it doesn't respect the following condition : objectGID < 0");
            powerRate = reader.ReadShort();
            overMax = reader.ReadBoolean();
            var limit = reader.ReadUShort();
            effects = new Types.ObjectEffect[limit];
            for (int i = 0; i < limit; i++)
            {
                 (effects as Types.ObjectEffect[])[i] = Types.ProtocolTypeManager.GetInstance<Types.ObjectEffect>(reader.ReadShort());
                 (effects as Types.ObjectEffect[])[i].Deserialize(reader);
            }
            objectUID = reader.ReadInt();
            if (objectUID < 0)
                throw new Exception("Forbidden value on objectUID = " + objectUID + ", it doesn't respect the following condition : objectUID < 0");
            quantity = reader.ReadInt();
            if (quantity < 0)
                throw new Exception("Forbidden value on quantity = " + quantity + ", it doesn't respect the following condition : quantity < 0");
            objectPrice = reader.ReadInt();
            if (objectPrice < 0)
                throw new Exception("Forbidden value on objectPrice = " + objectPrice + ", it doesn't respect the following condition : objectPrice < 0");
            publicPrice = reader.ReadInt();
            if (publicPrice < 0)
                throw new Exception("Forbidden value on publicPrice = " + publicPrice + ", it doesn't respect the following condition : publicPrice < 0");
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(short) + sizeof(short) + sizeof(bool) + sizeof(short) + effects.Sum(x => x.GetSerializationSize()) + sizeof(int) + sizeof(int) + sizeof(int) + sizeof(int);
        }
        
    }
    
}