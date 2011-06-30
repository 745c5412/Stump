// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'FightTeamMemberCharacterInformations.xml' the '30/06/2011 11:40:22'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
	public class FightTeamMemberCharacterInformations : FightTeamMemberInformations
	{
		public const uint Id = 13;
		public override short TypeId
		{
			get
			{
				return 13;
			}
		}
		
		public string name;
		public short level;
		
		public FightTeamMemberCharacterInformations()
		{
		}
		
		public FightTeamMemberCharacterInformations(int id, string name, short level)
			 : base(id)
		{
			this.name = name;
			this.level = level;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			base.Serialize(writer);
			writer.WriteUTF(name);
			writer.WriteShort(level);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			base.Deserialize(reader);
			name = reader.ReadUTF();
			level = reader.ReadShort();
			if ( level < 0 )
			{
				throw new Exception("Forbidden value on level = " + level + ", it doesn't respect the following condition : level < 0");
			}
		}
	}
}
