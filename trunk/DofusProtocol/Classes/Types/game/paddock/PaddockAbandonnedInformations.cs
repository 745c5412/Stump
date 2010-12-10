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
	
	public class PaddockAbandonnedInformations : PaddockBuyableInformations
	{
		public const uint protocolId = 133;
		public uint guildId = 0;
		
		public PaddockAbandonnedInformations()
		{
		}
		
		public PaddockAbandonnedInformations(uint arg1, uint arg2, uint arg3, uint arg4)
			: this()
		{
			initPaddockAbandonnedInformations(arg1, arg2, arg3, arg4);
		}
		
		public override uint getTypeId()
		{
			return 133;
		}
		
		public PaddockAbandonnedInformations initPaddockAbandonnedInformations(uint arg1 = 0, uint arg2 = 0, uint arg3 = 0, uint arg4 = 0)
		{
			base.initPaddockBuyableInformations(arg1, arg2, arg3);
			this.guildId = arg4;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.guildId = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_PaddockAbandonnedInformations(arg1);
		}
		
		public void serializeAs_PaddockAbandonnedInformations(BigEndianWriter arg1)
		{
			base.serializeAs_PaddockBuyableInformations(arg1);
			if ( this.guildId < 0 )
			{
				throw new Exception("Forbidden value (" + this.guildId + ") on element guildId.");
			}
			arg1.WriteInt((int)this.guildId);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PaddockAbandonnedInformations(arg1);
		}
		
		public void deserializeAs_PaddockAbandonnedInformations(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.guildId = (uint)arg1.ReadInt();
			if ( this.guildId < 0 )
			{
				throw new Exception("Forbidden value (" + this.guildId + ") on element of PaddockAbandonnedInformations.guildId.");
			}
		}
		
	}
}
