// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'GameMapMovementRequestMessage.xml' the '30/06/2011 11:40:11'
using System;
using Stump.Core.IO;
using System.Collections.Generic;
using System.Linq;

namespace Stump.DofusProtocol.Messages
{
	public class GameMapMovementRequestMessage : Message
	{
		public const uint Id = 950;
		public override uint MessageId
		{
			get
			{
				return 950;
			}
		}
		
		public IEnumerable<short> keyMovements;
		public int mapId;
		
		public GameMapMovementRequestMessage()
		{
		}
		
		public GameMapMovementRequestMessage(IEnumerable<short> keyMovements, int mapId)
		{
			this.keyMovements = keyMovements;
			this.mapId = mapId;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteUShort((ushort)keyMovements.Count());
			foreach (var entry in keyMovements)
			{
				writer.WriteShort(entry);
			}
			writer.WriteInt(mapId);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			int limit = reader.ReadUShort();
			keyMovements = new short[limit];
			for (int i = 0; i < limit; i++)
			{
				(keyMovements as short[])[i] = reader.ReadShort();
			}
			mapId = reader.ReadInt();
			if ( mapId < 0 )
			{
				throw new Exception("Forbidden value on mapId = " + mapId + ", it doesn't respect the following condition : mapId < 0");
			}
		}
	}
}
