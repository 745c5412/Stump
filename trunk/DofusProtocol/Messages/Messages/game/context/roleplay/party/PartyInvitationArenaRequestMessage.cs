// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'PartyInvitationArenaRequestMessage.xml' the '05/11/2011 17:25:52'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class PartyInvitationArenaRequestMessage : PartyInvitationRequestMessage
	{
		public const uint Id = 6283;
		public override uint MessageId
		{
			get
			{
				return 6283;
			}
		}
		
		
		public PartyInvitationArenaRequestMessage()
		{
		}
		
		public PartyInvitationArenaRequestMessage(string name)
			 : base(name)
		{
		}
		
		public override void Serialize(IDataWriter writer)
		{
			base.Serialize(writer);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			base.Deserialize(reader);
		}
	}
}