// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'ChallengeFightJoinRefusedMessage.xml' the '30/06/2011 11:40:16'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class ChallengeFightJoinRefusedMessage : Message
	{
		public const uint Id = 5908;
		public override uint MessageId
		{
			get
			{
				return 5908;
			}
		}
		
		public int playerId;
		public byte reason;
		
		public ChallengeFightJoinRefusedMessage()
		{
		}
		
		public ChallengeFightJoinRefusedMessage(int playerId, byte reason)
		{
			this.playerId = playerId;
			this.reason = reason;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteInt(playerId);
			writer.WriteByte(reason);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			playerId = reader.ReadInt();
			if ( playerId < 0 )
			{
				throw new Exception("Forbidden value on playerId = " + playerId + ", it doesn't respect the following condition : playerId < 0");
			}
			reason = reader.ReadByte();
		}
	}
}
