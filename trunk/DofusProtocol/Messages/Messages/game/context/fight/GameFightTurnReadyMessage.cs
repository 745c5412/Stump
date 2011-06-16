// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'GameFightTurnReadyMessage.xml' the '15/06/2011 01:38:48'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class GameFightTurnReadyMessage : Message
	{
		public const uint Id = 716;
		public override uint MessageId
		{
			get
			{
				return 716;
			}
		}
		
		public bool isReady;
		
		public GameFightTurnReadyMessage()
		{
		}
		
		public GameFightTurnReadyMessage(bool isReady)
		{
			this.isReady = isReady;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteBoolean(isReady);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			isReady = reader.ReadBoolean();
		}
	}
}
