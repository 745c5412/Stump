// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'GuildLeftMessage.xml' the '30/06/2011 11:40:16'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class GuildLeftMessage : Message
	{
		public const uint Id = 5562;
		public override uint MessageId
		{
			get
			{
				return 5562;
			}
		}
		
		
		public override void Serialize(IDataWriter writer)
		{
		}
		
		public override void Deserialize(IDataReader reader)
		{
		}
	}
}
