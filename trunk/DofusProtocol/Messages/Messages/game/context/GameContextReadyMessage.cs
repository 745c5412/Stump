// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'GameContextReadyMessage.xml' the '15/06/2011 01:38:46'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class GameContextReadyMessage : Message
	{
		public const uint Id = 6071;
		public override uint MessageId
		{
			get
			{
				return 6071;
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
