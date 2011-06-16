// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'CharacterNameSuggestionSuccessMessage.xml' the '15/06/2011 01:38:44'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class CharacterNameSuggestionSuccessMessage : Message
	{
		public const uint Id = 5544;
		public override uint MessageId
		{
			get
			{
				return 5544;
			}
		}
		
		public string suggestion;
		
		public CharacterNameSuggestionSuccessMessage()
		{
		}
		
		public CharacterNameSuggestionSuccessMessage(string suggestion)
		{
			this.suggestion = suggestion;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteUTF(suggestion);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			suggestion = reader.ReadUTF();
		}
	}
}
