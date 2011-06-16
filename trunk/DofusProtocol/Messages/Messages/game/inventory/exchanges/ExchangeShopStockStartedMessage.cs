// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'ExchangeShopStockStartedMessage.xml' the '15/06/2011 01:39:04'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class ExchangeShopStockStartedMessage : Message
	{
		public const uint Id = 5910;
		public override uint MessageId
		{
			get
			{
				return 5910;
			}
		}
		
		public Types.ObjectItemToSell[] objectsInfos;
		
		public ExchangeShopStockStartedMessage()
		{
		}
		
		public ExchangeShopStockStartedMessage(Types.ObjectItemToSell[] objectsInfos)
		{
			this.objectsInfos = objectsInfos;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteUShort((ushort)objectsInfos.Length);
			for (int i = 0; i < objectsInfos.Length; i++)
			{
				objectsInfos[i].Serialize(writer);
			}
		}
		
		public override void Deserialize(IDataReader reader)
		{
			int limit = reader.ReadUShort();
			objectsInfos = new Types.ObjectItemToSell[limit];
			for (int i = 0; i < limit; i++)
			{
				objectsInfos[i] = new Types.ObjectItemToSell();
				objectsInfos[i].Deserialize(reader);
			}
		}
	}
}
