// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'ObjectGroundAddedMessage.xml' the '15/06/2011 01:38:53'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class ObjectGroundAddedMessage : Message
	{
		public const uint Id = 3017;
		public override uint MessageId
		{
			get
			{
				return 3017;
			}
		}
		
		public short cellId;
		public short objectGID;
		
		public ObjectGroundAddedMessage()
		{
		}
		
		public ObjectGroundAddedMessage(short cellId, short objectGID)
		{
			this.cellId = cellId;
			this.objectGID = objectGID;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteShort(cellId);
			writer.WriteShort(objectGID);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			cellId = reader.ReadShort();
			if ( cellId < 0 || cellId > 559 )
			{
				throw new Exception("Forbidden value on cellId = " + cellId + ", it doesn't respect the following condition : cellId < 0 || cellId > 559");
			}
			objectGID = reader.ReadShort();
			if ( objectGID < 0 )
			{
				throw new Exception("Forbidden value on objectGID = " + objectGID + ", it doesn't respect the following condition : objectGID < 0");
			}
		}
	}
}
