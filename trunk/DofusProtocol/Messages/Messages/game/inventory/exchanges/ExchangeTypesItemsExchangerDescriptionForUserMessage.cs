// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'ExchangeTypesItemsExchangerDescriptionForUserMessage.xml' the '15/06/2011 01:39:05'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class ExchangeTypesItemsExchangerDescriptionForUserMessage : Message
	{
		public const uint Id = 5752;
		public override uint MessageId
		{
			get
			{
				return 5752;
			}
		}
		
		public Types.BidExchangerObjectInfo[] itemTypeDescriptions;
		
		public ExchangeTypesItemsExchangerDescriptionForUserMessage()
		{
		}
		
		public ExchangeTypesItemsExchangerDescriptionForUserMessage(Types.BidExchangerObjectInfo[] itemTypeDescriptions)
		{
			this.itemTypeDescriptions = itemTypeDescriptions;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteUShort((ushort)itemTypeDescriptions.Length);
			for (int i = 0; i < itemTypeDescriptions.Length; i++)
			{
				itemTypeDescriptions[i].Serialize(writer);
			}
		}
		
		public override void Deserialize(IDataReader reader)
		{
			int limit = reader.ReadUShort();
			itemTypeDescriptions = new Types.BidExchangerObjectInfo[limit];
			for (int i = 0; i < limit; i++)
			{
				itemTypeDescriptions[i] = new Types.BidExchangerObjectInfo();
				itemTypeDescriptions[i].Deserialize(reader);
			}
		}
	}
}
