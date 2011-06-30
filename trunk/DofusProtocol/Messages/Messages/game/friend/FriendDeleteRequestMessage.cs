// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'FriendDeleteRequestMessage.xml' the '30/06/2011 11:40:16'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class FriendDeleteRequestMessage : Message
	{
		public const uint Id = 5603;
		public override uint MessageId
		{
			get
			{
				return 5603;
			}
		}
		
		public string name;
		
		public FriendDeleteRequestMessage()
		{
		}
		
		public FriendDeleteRequestMessage(string name)
		{
			this.name = name;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteUTF(name);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			name = reader.ReadUTF();
		}
	}
}
