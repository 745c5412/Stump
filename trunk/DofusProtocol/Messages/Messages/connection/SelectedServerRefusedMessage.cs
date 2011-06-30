// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'SelectedServerRefusedMessage.xml' the '30/06/2011 11:40:08'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class SelectedServerRefusedMessage : Message
	{
		public const uint Id = 41;
		public override uint MessageId
		{
			get
			{
				return 41;
			}
		}
		
		public short serverId;
		public byte error;
		public byte serverStatus;
		
		public SelectedServerRefusedMessage()
		{
		}
		
		public SelectedServerRefusedMessage(short serverId, byte error, byte serverStatus)
		{
			this.serverId = serverId;
			this.error = error;
			this.serverStatus = serverStatus;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteShort(serverId);
			writer.WriteByte(error);
			writer.WriteByte(serverStatus);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			serverId = reader.ReadShort();
			error = reader.ReadByte();
			if ( error < 0 )
			{
				throw new Exception("Forbidden value on error = " + error + ", it doesn't respect the following condition : error < 0");
			}
			serverStatus = reader.ReadByte();
			if ( serverStatus < 0 )
			{
				throw new Exception("Forbidden value on serverStatus = " + serverStatus + ", it doesn't respect the following condition : serverStatus < 0");
			}
		}
	}
}
