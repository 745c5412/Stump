// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'ConsoleMessage.xml' the '30/06/2011 11:40:08'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class ConsoleMessage : Message
	{
		public const uint Id = 75;
		public override uint MessageId
		{
			get
			{
				return 75;
			}
		}
		
		public byte type;
		public string content;
		
		public ConsoleMessage()
		{
		}
		
		public ConsoleMessage(byte type, string content)
		{
			this.type = type;
			this.content = content;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteByte(type);
			writer.WriteUTF(content);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			type = reader.ReadByte();
			if ( type < 0 )
			{
				throw new Exception("Forbidden value on type = " + type + ", it doesn't respect the following condition : type < 0");
			}
			content = reader.ReadUTF();
		}
	}
}
