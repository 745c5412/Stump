// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'ChannelEnablingChangeMessage.xml' the '30/06/2011 11:40:11'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class ChannelEnablingChangeMessage : Message
	{
		public const uint Id = 891;
		public override uint MessageId
		{
			get
			{
				return 891;
			}
		}
		
		public byte channel;
		public bool enable;
		
		public ChannelEnablingChangeMessage()
		{
		}
		
		public ChannelEnablingChangeMessage(byte channel, bool enable)
		{
			this.channel = channel;
			this.enable = enable;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteByte(channel);
			writer.WriteBoolean(enable);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			channel = reader.ReadByte();
			if ( channel < 0 )
			{
				throw new Exception("Forbidden value on channel = " + channel + ", it doesn't respect the following condition : channel < 0");
			}
			enable = reader.ReadBoolean();
		}
	}
}
