// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'PaddockRemoveItemRequestMessage.xml' the '30/06/2011 11:40:12'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class PaddockRemoveItemRequestMessage : Message
	{
		public const uint Id = 5958;
		public override uint MessageId
		{
			get
			{
				return 5958;
			}
		}
		
		public short cellId;
		
		public PaddockRemoveItemRequestMessage()
		{
		}
		
		public PaddockRemoveItemRequestMessage(short cellId)
		{
			this.cellId = cellId;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteShort(cellId);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			cellId = reader.ReadShort();
			if ( cellId < 0 || cellId > 559 )
			{
				throw new Exception("Forbidden value on cellId = " + cellId + ", it doesn't respect the following condition : cellId < 0 || cellId > 559");
			}
		}
	}
}
