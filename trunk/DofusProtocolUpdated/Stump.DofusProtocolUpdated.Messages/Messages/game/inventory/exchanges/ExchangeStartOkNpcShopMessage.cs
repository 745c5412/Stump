

// Generated on 12/12/2013 16:57:18
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeStartOkNpcShopMessage : Message
    {
        public const uint Id = 5761;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int npcSellerId;
        public int tokenId;
        public IEnumerable<Types.ObjectItemToSellInNpcShop> objectsInfos;
        
        public ExchangeStartOkNpcShopMessage()
        {
        }
        
        public ExchangeStartOkNpcShopMessage(int npcSellerId, int tokenId, IEnumerable<Types.ObjectItemToSellInNpcShop> objectsInfos)
        {
            this.npcSellerId = npcSellerId;
            this.tokenId = tokenId;
            this.objectsInfos = objectsInfos;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(npcSellerId);
            writer.WriteInt(tokenId);
            writer.WriteUShort((ushort)objectsInfos.Count());
            foreach (var entry in objectsInfos)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            npcSellerId = reader.ReadInt();
            tokenId = reader.ReadInt();
            if (tokenId < 0)
                throw new Exception("Forbidden value on tokenId = " + tokenId + ", it doesn't respect the following condition : tokenId < 0");
            var limit = reader.ReadUShort();
            objectsInfos = new Types.ObjectItemToSellInNpcShop[limit];
            for (int i = 0; i < limit; i++)
            {
                 (objectsInfos as Types.ObjectItemToSellInNpcShop[])[i] = new Types.ObjectItemToSellInNpcShop();
                 (objectsInfos as Types.ObjectItemToSellInNpcShop[])[i].Deserialize(reader);
            }
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(int) + sizeof(int) + sizeof(short) + objectsInfos.Sum(x => x.GetSerializationSize());
        }
        
    }
    
}