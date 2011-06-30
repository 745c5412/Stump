// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'BasicWhoIsMessage.xml' the '30/06/2011 11:40:10'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class BasicWhoIsMessage : Message
	{
		public const uint Id = 180;
		public override uint MessageId
		{
			get
			{
				return 180;
			}
		}
		
		public bool self;
		public byte position;
		public string accountNickname;
		public string characterName;
		public short areaId;
		
		public BasicWhoIsMessage()
		{
		}
		
		public BasicWhoIsMessage(bool self, byte position, string accountNickname, string characterName, short areaId)
		{
			this.self = self;
			this.position = position;
			this.accountNickname = accountNickname;
			this.characterName = characterName;
			this.areaId = areaId;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteBoolean(self);
			writer.WriteByte(position);
			writer.WriteUTF(accountNickname);
			writer.WriteUTF(characterName);
			writer.WriteShort(areaId);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			self = reader.ReadBoolean();
			position = reader.ReadByte();
			accountNickname = reader.ReadUTF();
			characterName = reader.ReadUTF();
			areaId = reader.ReadShort();
		}
	}
}
