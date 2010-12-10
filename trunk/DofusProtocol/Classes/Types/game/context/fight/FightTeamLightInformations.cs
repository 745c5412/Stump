// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class FightTeamLightInformations : AbstractFightTeamInformations
	{
		public const uint protocolId = 115;
		public uint teamMembersCount = 0;
		
		public FightTeamLightInformations()
		{
		}
		
		public FightTeamLightInformations(uint arg1, int arg2, int arg3, uint arg4, uint arg5)
			: this()
		{
			initFightTeamLightInformations(arg1, arg2, arg3, arg4, arg5);
		}
		
		public override uint getTypeId()
		{
			return 115;
		}
		
		public FightTeamLightInformations initFightTeamLightInformations(uint arg1 = 2, int arg2 = 0, int arg3 = 0, uint arg4 = 0, uint arg5 = 0)
		{
			base.initAbstractFightTeamInformations(arg1, arg2, arg3, arg4);
			this.teamMembersCount = arg5;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.teamMembersCount = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_FightTeamLightInformations(arg1);
		}
		
		public void serializeAs_FightTeamLightInformations(BigEndianWriter arg1)
		{
			base.serializeAs_AbstractFightTeamInformations(arg1);
			if ( this.teamMembersCount < 0 )
			{
				throw new Exception("Forbidden value (" + this.teamMembersCount + ") on element teamMembersCount.");
			}
			arg1.WriteByte((byte)this.teamMembersCount);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_FightTeamLightInformations(arg1);
		}
		
		public void deserializeAs_FightTeamLightInformations(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.teamMembersCount = (uint)arg1.ReadByte();
			if ( this.teamMembersCount < 0 )
			{
				throw new Exception("Forbidden value (" + this.teamMembersCount + ") on element of FightTeamLightInformations.teamMembersCount.");
			}
		}
		
	}
}
