// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'ObjectGroundRemovedMultipleMessage.xml' the '15/06/2011 01:38:53'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class ObjectGroundRemovedMultipleMessage : Message
	{
		public const uint Id = 5944;
		public override uint MessageId
		{
			get
			{
				return 5944;
			}
		}
		
		public short[] cells;
		
		public ObjectGroundRemovedMultipleMessage()
		{
		}
		
		public ObjectGroundRemovedMultipleMessage(short[] cells)
		{
			this.cells = cells;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteUShort((ushort)cells.Length);
			for (int i = 0; i < cells.Length; i++)
			{
				writer.WriteShort(cells[i]);
			}
		}
		
		public override void Deserialize(IDataReader reader)
		{
			int limit = reader.ReadUShort();
			cells = new short[limit];
			for (int i = 0; i < limit; i++)
			{
				cells[i] = reader.ReadShort();
			}
		}
	}
}
