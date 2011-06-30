// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'SpellUpgradeSuccessMessage.xml' the '30/06/2011 11:40:15'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class SpellUpgradeSuccessMessage : Message
	{
		public const uint Id = 1201;
		public override uint MessageId
		{
			get
			{
				return 1201;
			}
		}
		
		public int spellId;
		public byte spellLevel;
		
		public SpellUpgradeSuccessMessage()
		{
		}
		
		public SpellUpgradeSuccessMessage(int spellId, byte spellLevel)
		{
			this.spellId = spellId;
			this.spellLevel = spellLevel;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteInt(spellId);
			writer.WriteByte(spellLevel);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			spellId = reader.ReadInt();
			spellLevel = reader.ReadByte();
			if ( spellLevel < 1 || spellLevel > 6 )
			{
				throw new Exception("Forbidden value on spellLevel = " + spellLevel + ", it doesn't respect the following condition : spellLevel < 1 || spellLevel > 6");
			}
		}
	}
}
