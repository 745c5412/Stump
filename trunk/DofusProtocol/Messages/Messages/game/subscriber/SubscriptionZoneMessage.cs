// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'SubscriptionZoneMessage.xml' the '30/06/2011 11:40:21'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class SubscriptionZoneMessage : Message
	{
		public const uint Id = 5573;
		public override uint MessageId
		{
			get
			{
				return 5573;
			}
		}
		
		public bool active;
		
		public SubscriptionZoneMessage()
		{
		}
		
		public SubscriptionZoneMessage(bool active)
		{
			this.active = active;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteBoolean(active);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			active = reader.ReadBoolean();
		}
	}
}
