// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'InventoryPresetDeleteResultMessage.xml' the '30/06/2011 11:40:20'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class InventoryPresetDeleteResultMessage : Message
	{
		public const uint Id = 6173;
		public override uint MessageId
		{
			get
			{
				return 6173;
			}
		}
		
		public byte presetId;
		public byte code;
		
		public InventoryPresetDeleteResultMessage()
		{
		}
		
		public InventoryPresetDeleteResultMessage(byte presetId, byte code)
		{
			this.presetId = presetId;
			this.code = code;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteByte(presetId);
			writer.WriteByte(code);
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
		}
	}
}
