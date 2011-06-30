// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'PartyAcceptInvitationMessage.xml' the '30/06/2011 11:40:15'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class PartyAcceptInvitationMessage : Message
	{
		public const uint Id = 5580;
		public override uint MessageId
		{
			get
			{
				return 5580;
			}
		}
		
		public int partyId;
		
		public PartyAcceptInvitationMessage()
		{
		}
		
		public PartyAcceptInvitationMessage(int partyId)
		{
			this.partyId = partyId;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteInt(partyId);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			partyId = reader.ReadInt();
			if ( partyId < 0 )
			{
				throw new Exception("Forbidden value on partyId = " + partyId + ", it doesn't respect the following condition : partyId < 0");
			}
		}
	}
}
