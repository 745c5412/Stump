// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'InventoryPresetUseResultMessage.xml' the '15/06/2011 01:39:07'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class InventoryPresetUseResultMessage : Message
	{
		public const uint Id = 6163;
		public override uint MessageId
		{
			get
			{
				return 6163;
			}
		}
		
		public byte presetId;
		public byte code;
		public byte[] unlinkedPosition;
		
		public InventoryPresetUseResultMessage()
		{
		}
		
		public InventoryPresetUseResultMessage(byte presetId, byte code, byte[] unlinkedPosition)
		{
			this.presetId = presetId;
			this.code = code;
			this.unlinkedPosition = unlinkedPosition;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteByte(presetId);
			writer.WriteByte(code);
			writer.WriteUShort((ushort)unlinkedPosition.Length);
			for (int i = 0; i < unlinkedPosition.Length; i++)
			{
				writer.WriteByte(unlinkedPosition[i]);
			}
		}
		
		public override void Deserialize(IDataReader reader)
		{
			presetId = reader.ReadByte();
			if ( presetId < 0 )
			{
				throw new Exception("Forbidden value on presetId = " + presetId + ", it doesn't respect the following condition : presetId < 0");
			}
			code = reader.ReadByte();
			if ( code < 0 )
			{
				throw new Exception("Forbidden value on code = " + code + ", it doesn't respect the following condition : code < 0");
			}
			int limit = reader.ReadUShort();
			unlinkedPosition = new byte[limit];
			for (int i = 0; i < limit; i++)
			{
				unlinkedPosition[i] = reader.ReadByte();
			}
		}
	}
}
