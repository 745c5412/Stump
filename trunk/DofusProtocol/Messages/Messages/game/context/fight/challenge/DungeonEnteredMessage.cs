// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'DungeonEnteredMessage.xml' the '15/06/2011 01:38:48'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class DungeonEnteredMessage : Message
	{
		public const uint Id = 6152;
		public override uint MessageId
		{
			get
			{
				return 6152;
			}
		}
		
		public int dungeonId;
		
		public DungeonEnteredMessage()
		{
		}
		
		public DungeonEnteredMessage(int dungeonId)
		{
			this.dungeonId = dungeonId;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteInt(dungeonId);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			dungeonId = reader.ReadInt();
			if ( dungeonId < 0 )
			{
				throw new Exception("Forbidden value on dungeonId = " + dungeonId + ", it doesn't respect the following condition : dungeonId < 0");
			}
		}
	}
}
