// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'TextInformationMessage.xml' the '15/06/2011 01:38:43'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class TextInformationMessage : Message
	{
		public const uint Id = 780;
		public override uint MessageId
		{
			get
			{
				return 780;
			}
		}
		
		public byte msgType;
		public short msgId;
		public string[] parameters;
		
		public TextInformationMessage()
		{
		}
		
		public TextInformationMessage(byte msgType, short msgId, string[] parameters)
		{
			this.msgType = msgType;
			this.msgId = msgId;
			this.parameters = parameters;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteByte(msgType);
			writer.WriteShort(msgId);
			writer.WriteUShort((ushort)parameters.Length);
			for (int i = 0; i < parameters.Length; i++)
			{
				writer.WriteUTF(parameters[i]);
			}
		}
		
		public override void Deserialize(IDataReader reader)
		{
			msgType = reader.ReadByte();
			if ( msgType < 0 )
			{
				throw new Exception("Forbidden value on msgType = " + msgType + ", it doesn't respect the following condition : msgType < 0");
			}
			msgId = reader.ReadShort();
			if ( msgId < 0 )
			{
				throw new Exception("Forbidden value on msgId = " + msgId + ", it doesn't respect the following condition : msgId < 0");
			}
			int limit = reader.ReadUShort();
			parameters = new string[limit];
			for (int i = 0; i < limit; i++)
			{
				parameters[i] = reader.ReadUTF();
			}
		}
	}
}
