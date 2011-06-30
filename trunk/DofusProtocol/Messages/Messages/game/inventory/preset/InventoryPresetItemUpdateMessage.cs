// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'InventoryPresetItemUpdateMessage.xml' the '30/06/2011 11:40:20'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class InventoryPresetItemUpdateMessage : Message
	{
		public const uint Id = 6168;
		public override uint MessageId
		{
			get
			{
				return 6168;
			}
		}
		
		public byte presetId;
		public Types.PresetItem presetItem;
		
		public InventoryPresetItemUpdateMessage()
		{
		}
		
		public InventoryPresetItemUpdateMessage(byte presetId, Types.PresetItem presetItem)
		{
			this.presetId = presetId;
			this.presetItem = presetItem;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteByte(presetId);
			presetItem.Serialize(writer);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			presetId = reader.ReadByte();
			if ( presetId < 0 )
			{
				throw new Exception("Forbidden value on presetId = " + presetId + ", it doesn't respect the following condition : presetId < 0");
			}
			presetItem = new Types.PresetItem();
			presetItem.Deserialize(reader);
		}
	}
}
