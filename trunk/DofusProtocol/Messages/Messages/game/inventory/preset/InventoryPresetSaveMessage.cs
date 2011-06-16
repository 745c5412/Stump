// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'InventoryPresetSaveMessage.xml' the '15/06/2011 01:39:07'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class InventoryPresetSaveMessage : Message
	{
		public const uint Id = 6165;
		public override uint MessageId
		{
			get
			{
				return 6165;
			}
		}
		
		public byte presetId;
		public byte symbolId;
		public bool saveEquipment;
		
		public InventoryPresetSaveMessage()
		{
		}
		
		public InventoryPresetSaveMessage(byte presetId, byte symbolId, bool saveEquipment)
		{
			this.presetId = presetId;
			this.symbolId = symbolId;
			this.saveEquipment = saveEquipment;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteByte(presetId);
			writer.WriteByte(symbolId);
			writer.WriteBoolean(saveEquipment);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			presetId = reader.ReadByte();
			if ( presetId < 0 )
			{
				throw new Exception("Forbidden value on presetId = " + presetId + ", it doesn't respect the following condition : presetId < 0");
			}
			symbolId = reader.ReadByte();
			if ( symbolId < 0 )
			{
				throw new Exception("Forbidden value on symbolId = " + symbolId + ", it doesn't respect the following condition : symbolId < 0");
			}
			saveEquipment = reader.ReadBoolean();
		}
	}
}
