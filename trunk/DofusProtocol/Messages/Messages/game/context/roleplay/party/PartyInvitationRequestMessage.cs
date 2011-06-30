// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'PartyInvitationRequestMessage.xml' the '30/06/2011 11:40:15'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class PartyInvitationRequestMessage : Message
	{
		public const uint Id = 5585;
		public override uint MessageId
		{
			get
			{
				return 5585;
			}
		}
		
		public string name;
		
		public PartyInvitationRequestMessage()
		{
		}
		
		public PartyInvitationRequestMessage(string name)
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
