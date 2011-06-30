// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'TeleportRequestMessage.xml' the '30/06/2011 11:40:17'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class TeleportRequestMessage : Message
	{
		public const uint Id = 5961;
		public override uint MessageId
		{
			get
			{
				return 5961;
			}
		}
		
		public byte teleporterType;
		public int mapId;
		
		public TeleportRequestMessage()
		{
		}
		
		public TeleportRequestMessage(byte teleporterType, int mapId)
		{
			this.teleporterType = teleporterType;
			this.mapId = mapId;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteByte(teleporterType);
			writer.WriteInt(mapId);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			teleporterType = reader.ReadByte();
			if ( teleporterType < 0 )
			{
				throw new Exception("Forbidden value on teleporterType = " + teleporterType + ", it doesn't respect the following condition : teleporterType < 0");
			}
			mapId = reader.ReadInt();
			if ( mapId < 0 )
			{
				throw new Exception("Forbidden value on mapId = " + mapId + ", it doesn't respect the following condition : mapId < 0");
			}
		}
	}
}
