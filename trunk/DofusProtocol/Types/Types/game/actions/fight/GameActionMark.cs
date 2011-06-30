// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'GameActionMark.xml' the '30/06/2011 11:40:22'
using System;
using Stump.Core.IO;
using System.Collections.Generic;
using System.Linq;

namespace Stump.DofusProtocol.Types
{
	public class GameActionMark
	{
		public const uint Id = 351;
		public virtual short TypeId
		{
			get
			{
				return 351;
			}
		}
		
		public int markAuthorId;
		public int markSpellId;
		public short markId;
		public byte markType;
		public IEnumerable<Types.GameActionMarkedCell> cells;
		
		public GameActionMark()
		{
		}
		
		public GameActionMark(int markAuthorId, int markSpellId, short markId, byte markType, IEnumerable<Types.GameActionMarkedCell> cells)
		{
			this.markAuthorId = markAuthorId;
			this.markSpellId = markSpellId;
			this.markId = markId;
			this.markType = markType;
			this.cells = cells;
		}
		
		public virtual void Serialize(IDataWriter writer)
		{
			writer.WriteInt(markAuthorId);
			writer.WriteInt(markSpellId);
			writer.WriteShort(markId);
			writer.WriteByte(markType);
			writer.WriteUShort((ushort)cells.Count());
			foreach (var entry in cells)
			{
				entry.Serialize(writer);
			}
		}
		
		public virtual void Deserialize(IDataReader reader)
		{
			markAuthorId = reader.ReadInt();
			markSpellId = reader.ReadInt();
			if ( markSpellId < 0 )
			{
				throw new Exception("Forbidden value on markSpellId = " + markSpellId + ", it doesn't respect the following condition : markSpellId < 0");
			}
			markId = reader.ReadShort();
			markType = reader.ReadByte();
			int limit = reader.ReadUShort();
			cells = new Types.GameActionMarkedCell[limit];
			for (int i = 0; i < limit; i++)
			{
				(cells as GameActionMarkedCell[])[i] = new Types.GameActionMarkedCell();
				(cells as Types.GameActionMarkedCell[])[i].Deserialize(reader);
			}
		}
	}
}
