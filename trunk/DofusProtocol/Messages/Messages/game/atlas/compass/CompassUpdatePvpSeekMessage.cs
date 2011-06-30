// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'CompassUpdatePvpSeekMessage.xml' the '30/06/2011 11:40:10'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class CompassUpdatePvpSeekMessage : CompassUpdateMessage
	{
		public const uint Id = 6013;
		public override uint MessageId
		{
			get
			{
				return 6013;
			}
		}
		
		public int memberId;
		public string memberName;
		
		public CompassUpdatePvpSeekMessage()
		{
		}
		
		public CompassUpdatePvpSeekMessage(byte type, short worldX, short worldY, int memberId, string memberName)
			 : base(type, worldX, worldY)
		{
			this.memberId = memberId;
			this.memberName = memberName;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			base.Serialize(writer);
			writer.WriteInt(memberId);
			writer.WriteUTF(memberName);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			base.Deserialize(reader);
			memberId = reader.ReadInt();
			if ( memberId < 0 )
			{
				throw new Exception("Forbidden value on memberId = " + memberId + ", it doesn't respect the following condition : memberId < 0");
			}
			memberName = reader.ReadUTF();
		}
	}
}
