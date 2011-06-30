// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'GameRolePlaySpellAnimMessage.xml' the '30/06/2011 11:40:15'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class GameRolePlaySpellAnimMessage : Message
	{
		public const uint Id = 6114;
		public override uint MessageId
		{
			get
			{
				return 6114;
			}
		}
		
		public int casterId;
		public short targetCellId;
		public short spellId;
		public byte spellLevel;
		
		public GameRolePlaySpellAnimMessage()
		{
		}
		
		public GameRolePlaySpellAnimMessage(int casterId, short targetCellId, short spellId, byte spellLevel)
		{
			this.casterId = casterId;
			this.targetCellId = targetCellId;
			this.spellId = spellId;
			this.spellLevel = spellLevel;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteInt(casterId);
			writer.WriteShort(targetCellId);
			writer.WriteShort(spellId);
			writer.WriteByte(spellLevel);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			casterId = reader.ReadInt();
			targetCellId = reader.ReadShort();
			if ( targetCellId < 0 || targetCellId > 559 )
			{
				throw new Exception("Forbidden value on targetCellId = " + targetCellId + ", it doesn't respect the following condition : targetCellId < 0 || targetCellId > 559");
			}
			spellId = reader.ReadShort();
			if ( spellId < 0 )
			{
				throw new Exception("Forbidden value on spellId = " + spellId + ", it doesn't respect the following condition : spellId < 0");
			}
			spellLevel = reader.ReadByte();
			if ( spellLevel < 1 || spellLevel > 6 )
			{
				throw new Exception("Forbidden value on spellLevel = " + spellLevel + ", it doesn't respect the following condition : spellLevel < 1 || spellLevel > 6");
			}
		}
	}
}
