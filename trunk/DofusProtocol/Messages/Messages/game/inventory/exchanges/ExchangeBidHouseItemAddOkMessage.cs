// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'ExchangeBidHouseItemAddOkMessage.xml' the '30/06/2011 11:40:17'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class ExchangeBidHouseItemAddOkMessage : Message
	{
		public const uint Id = 5945;
		public override uint MessageId
		{
			get
			{
				return 5945;
			}
		}
		
		public Types.ObjectItemToSellInBid itemInfo;
		
		public ExchangeBidHouseItemAddOkMessage()
		{
		}
		
		public ExchangeBidHouseItemAddOkMessage(Types.ObjectItemToSellInBid itemInfo)
		{
			this.itemInfo = itemInfo;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			itemInfo.Serialize(writer);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			itemInfo = new Types.ObjectItemToSellInBid();
			itemInfo.Deserialize(reader);
		}
	}
}
