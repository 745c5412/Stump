// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'BasicGuildInformations.xml' the '14/06/2011 11:32:47'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
	public class BasicGuildInformations
	{
		public const uint Id = 365;
		public short TypeId
		{
			get
			{
				return 365;
			}
		}
		
		public int guildId;
		public string guildName;
		
		public BasicGuildInformations()
		{
		}
		
		public BasicGuildInformations(int guildId, string guildName)
		{
			this.guildId = guildId;
			this.guildName = guildName;
		}
		
		public virtual void Serialize(IDataWriter writer)
		{
			writer.WriteInt(guildId);
			writer.WriteUTF(guildName);
		}
		
		public virtual void Deserialize(IDataReader reader)
		{
			guildId = reader.ReadInt();
			if ( guildId < 0 )
			{
				throw new Exception("Forbidden value on guildId = " + guildId + ", it doesn't respect the following condition : guildId < 0");
			}
			guildName = reader.ReadUTF();
		}
	}
}
