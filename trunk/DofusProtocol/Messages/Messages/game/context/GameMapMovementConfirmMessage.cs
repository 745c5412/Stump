// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'GameMapMovementConfirmMessage.xml' the '15/06/2011 01:38:47'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class GameMapMovementConfirmMessage : Message
	{
		public const uint Id = 952;
		public override uint MessageId
		{
			get
			{
				return 952;
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
