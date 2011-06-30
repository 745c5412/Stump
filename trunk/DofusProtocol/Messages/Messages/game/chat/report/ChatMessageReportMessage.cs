// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'ChatMessageReportMessage.xml' the '30/06/2011 11:40:11'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class ChatMessageReportMessage : Message
	{
		public const uint Id = 821;
		public override uint MessageId
		{
			get
			{
				return 821;
			}
		}
		
		public string senderName;
		public string content;
		public int timestamp;
		public byte channel;
		public string fingerprint;
		public byte reason;
		
		public ChatMessageReportMessage()
		{
		}
		
		public ChatMessageReportMessage(string senderName, string content, int timestamp, byte channel, string fingerprint, byte reason)
		{
			this.senderName = senderName;
			this.content = content;
			this.timestamp = timestamp;
			this.channel = channel;
			this.fingerprint = fingerprint;
			this.reason = reason;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteUTF(senderName);
			writer.WriteUTF(content);
			writer.WriteInt(timestamp);
			writer.WriteByte(channel);
			writer.WriteUTF(fingerprint);
			writer.WriteByte(reason);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			senderName = reader.ReadUTF();
			content = reader.ReadUTF();
			timestamp = reader.ReadInt();
			if ( timestamp < 0 )
			{
				throw new Exception("Forbidden value on timestamp = " + timestamp + ", it doesn't respect the following condition : timestamp < 0");
			}
			channel = reader.ReadByte();
			if ( channel < 0 )
			{
				throw new Exception("Forbidden value on channel = " + channel + ", it doesn't respect the following condition : channel < 0");
			}
			fingerprint = reader.ReadUTF();
			reason = reader.ReadByte();
			if ( reason < 0 )
			{
				throw new Exception("Forbidden value on reason = " + reason + ", it doesn't respect the following condition : reason < 0");
			}
		}
	}
}
