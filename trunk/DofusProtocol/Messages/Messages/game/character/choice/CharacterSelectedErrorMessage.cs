// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'CharacterSelectedErrorMessage.xml' the '30/06/2011 11:40:10'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class CharacterSelectedErrorMessage : Message
	{
		public const uint Id = 5836;
		public override uint MessageId
		{
			get
			{
				return 5836;
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
