// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'CharactersListRequestMessage.xml' the '15/06/2011 01:38:43'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class CharactersListRequestMessage : Message
	{
		public const uint Id = 150;
		public override uint MessageId
		{
			get
			{
				return 150;
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
