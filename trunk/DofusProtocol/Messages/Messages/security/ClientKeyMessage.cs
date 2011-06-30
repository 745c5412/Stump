// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'ClientKeyMessage.xml' the '30/06/2011 11:40:21'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class ClientKeyMessage : Message
	{
		public const uint Id = 5607;
		public override uint MessageId
		{
			get
			{
				return 5607;
			}
		}
		
		public string key;
		
		public ClientKeyMessage()
		{
		}
		
		public ClientKeyMessage(string key)
		{
			this.key = key;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteUTF(key);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			key = reader.ReadUTF();
		}
	}
}
