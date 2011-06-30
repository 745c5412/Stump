// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'GuildPaddockRemovedMessage.xml' the '30/06/2011 11:40:16'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class GuildPaddockRemovedMessage : Message
	{
		public const uint Id = 5955;
		public override uint MessageId
		{
			get
			{
				return 5955;
			}
		}
		
		public short worldX;
		public short worldY;
		
		public GuildPaddockRemovedMessage()
		{
		}
		
		public GuildPaddockRemovedMessage(short worldX, short worldY)
		{
			this.worldX = worldX;
			this.worldY = worldY;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteShort(worldX);
			writer.WriteShort(worldY);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			worldX = reader.ReadShort();
			if ( worldX < -255 || worldX > 255 )
			{
				throw new Exception("Forbidden value on worldX = " + worldX + ", it doesn't respect the following condition : worldX < -255 || worldX > 255");
			}
			worldY = reader.ReadShort();
			if ( worldY < -255 || worldY > 255 )
			{
				throw new Exception("Forbidden value on worldY = " + worldY + ", it doesn't respect the following condition : worldY < -255 || worldY > 255");
			}
		}
	}
}
