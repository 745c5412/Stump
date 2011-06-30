// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'SubscriptionLimitationMessage.xml' the '30/06/2011 11:40:21'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class SubscriptionLimitationMessage : Message
	{
		public const uint Id = 5542;
		public override uint MessageId
		{
			get
			{
				return 5542;
			}
		}
		
		public byte reason;
		
		public SubscriptionLimitationMessage()
		{
		}
		
		public SubscriptionLimitationMessage(byte reason)
		{
			this.reason = reason;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteByte(reason);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			reason = reader.ReadByte();
			if ( reason < 0 )
			{
				throw new Exception("Forbidden value on reason = " + reason + ", it doesn't respect the following condition : reason < 0");
			}
		}
	}
}
