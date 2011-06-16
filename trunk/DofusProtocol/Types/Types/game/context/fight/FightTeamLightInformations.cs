// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'FightTeamLightInformations.xml' the '14/06/2011 11:32:46'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
	public class FightTeamLightInformations : AbstractFightTeamInformations
	{
		public const uint Id = 115;
		public short TypeId
		{
			get
			{
				return 115;
			}
		}
		
		public byte teamMembersCount;
		
		public FightTeamLightInformations()
		{
		}
		
		public FightTeamLightInformations(byte teamId, int leaderId, byte teamSide, byte teamTypeId, byte teamMembersCount)
			 : base(teamId, leaderId, teamSide, teamTypeId)
		{
			this.teamMembersCount = teamMembersCount;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			base.Serialize(writer);
			writer.WriteByte(teamMembersCount);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			base.Deserialize(reader);
			teamMembersCount = reader.ReadByte();
			if ( teamMembersCount < 0 )
			{
				throw new Exception("Forbidden value on teamMembersCount = " + teamMembersCount + ", it doesn't respect the following condition : teamMembersCount < 0");
			}
		}
	}
}
