// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'CompassUpdatePartyMemberMessage.xml' the '30/06/2011 11:40:10'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class CompassUpdatePartyMemberMessage : CompassUpdateMessage
	{
		public const uint Id = 5589;
		public override uint MessageId
		{
			get
			{
				return 5589;
			}
		}
		
		public int memberId;
		
		public CompassUpdatePartyMemberMessage()
		{
		}
		
		public CompassUpdatePartyMemberMessage(byte type, short worldX, short worldY, int memberId)
			 : base(type, worldX, worldY)
		{
			this.memberId = memberId;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			base.Serialize(writer);
			writer.WriteInt(memberId);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			base.Deserialize(reader);
			memberId = reader.ReadInt();
			if ( memberId < 0 )
			{
				throw new Exception("Forbidden value on memberId = " + memberId + ", it doesn't respect the following condition : memberId < 0");
			}
		}
	}
}
