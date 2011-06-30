// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'HouseSellRequestMessage.xml' the '30/06/2011 11:40:14'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class HouseSellRequestMessage : Message
	{
		public const uint Id = 5697;
		public override uint MessageId
		{
			get
			{
				return 5697;
			}
		}
		
		public int amount;
		
		public HouseSellRequestMessage()
		{
		}
		
		public HouseSellRequestMessage(int amount)
		{
			this.amount = amount;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteInt(amount);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			amount = reader.ReadInt();
			if ( amount < 0 )
			{
				throw new Exception("Forbidden value on amount = " + amount + ", it doesn't respect the following condition : amount < 0");
			}
		}
	}
}
