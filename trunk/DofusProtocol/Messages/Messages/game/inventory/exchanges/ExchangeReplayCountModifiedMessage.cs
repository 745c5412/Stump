// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'ExchangeReplayCountModifiedMessage.xml' the '30/06/2011 11:40:18'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class ExchangeReplayCountModifiedMessage : Message
	{
		public const uint Id = 6023;
		public override uint MessageId
		{
			get
			{
				return 6023;
			}
		}
		
		public int count;
		
		public ExchangeReplayCountModifiedMessage()
		{
		}
		
		public ExchangeReplayCountModifiedMessage(int count)
		{
			this.count = count;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteInt(count);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			count = reader.ReadInt();
		}
	}
}
