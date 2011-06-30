// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'ExchangeKamaModifiedMessage.xml' the '30/06/2011 11:40:19'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class ExchangeKamaModifiedMessage : ExchangeObjectMessage
	{
		public const uint Id = 5521;
		public override uint MessageId
		{
			get
			{
				return 5521;
			}
		}
		
		public int quantity;
		
		public ExchangeKamaModifiedMessage()
		{
		}
		
		public ExchangeKamaModifiedMessage(bool remote, int quantity)
			 : base(remote)
		{
			this.quantity = quantity;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			base.Serialize(writer);
			writer.WriteInt(quantity);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			base.Deserialize(reader);
			quantity = reader.ReadInt();
			if ( quantity < 0 )
			{
				throw new Exception("Forbidden value on quantity = " + quantity + ", it doesn't respect the following condition : quantity < 0");
			}
		}
	}
}
