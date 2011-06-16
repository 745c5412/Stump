// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'AlignmentSubAreaUpdateMessage.xml' the '15/06/2011 01:39:09'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class AlignmentSubAreaUpdateMessage : Message
	{
		public const uint Id = 6057;
		public override uint MessageId
		{
			get
			{
				return 6057;
			}
		}
		
		public short subAreaId;
		public byte side;
		public bool quiet;
		
		public AlignmentSubAreaUpdateMessage()
		{
		}
		
		public AlignmentSubAreaUpdateMessage(short subAreaId, byte side, bool quiet)
		{
			this.subAreaId = subAreaId;
			this.side = side;
			this.quiet = quiet;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteShort(subAreaId);
			writer.WriteByte(side);
			writer.WriteBoolean(quiet);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			subAreaId = reader.ReadShort();
			if ( subAreaId < 0 )
			{
				throw new Exception("Forbidden value on subAreaId = " + subAreaId + ", it doesn't respect the following condition : subAreaId < 0");
			}
			side = reader.ReadByte();
			quiet = reader.ReadBoolean();
		}
	}
}
