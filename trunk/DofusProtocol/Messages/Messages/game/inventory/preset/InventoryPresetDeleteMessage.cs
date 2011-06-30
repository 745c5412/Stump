// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'InventoryPresetDeleteMessage.xml' the '30/06/2011 11:40:20'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class InventoryPresetDeleteMessage : Message
	{
		public const uint Id = 6169;
		public override uint MessageId
		{
			get
			{
				return 6169;
			}
		}
		
		public byte presetId;
		
		public InventoryPresetDeleteMessage()
		{
		}
		
		public InventoryPresetDeleteMessage(byte presetId)
		{
			this.presetId = presetId;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteByte(presetId);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			presetId = reader.ReadByte();
			if ( presetId < 0 )
			{
				throw new Exception("Forbidden value on presetId = " + presetId + ", it doesn't respect the following condition : presetId < 0");
			}
		}
	}
}
