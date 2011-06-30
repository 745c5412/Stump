// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'HouseGuildRightsChangeRequestMessage.xml' the '30/06/2011 11:40:14'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class HouseGuildRightsChangeRequestMessage : Message
	{
		public const uint Id = 5702;
		public override uint MessageId
		{
			get
			{
				return 5702;
			}
		}
		
		public uint rights;
		
		public HouseGuildRightsChangeRequestMessage()
		{
		}
		
		public HouseGuildRightsChangeRequestMessage(uint rights)
		{
			this.rights = rights;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteUInt(rights);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			rights = reader.ReadUInt();
			if ( rights < 0 || rights > 4294967295 )
			{
				throw new Exception("Forbidden value on rights = " + rights + ", it doesn't respect the following condition : rights < 0 || rights > 4294967295");
			}
		}
	}
}
