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
	
	public class InteractiveElementNamedSkill : InteractiveElementSkill
	{
		public const uint protocolId = 220;
		public uint nameId = 0;
		
		public InteractiveElementNamedSkill()
		{
		}
		
		public InteractiveElementNamedSkill(uint arg1, uint arg2, uint arg3)
			: this()
		{
			initInteractiveElementNamedSkill(arg1, arg2, arg3);
		}
		
		public override uint getTypeId()
		{
			return 220;
		}
		
		public InteractiveElementNamedSkill initInteractiveElementNamedSkill(uint arg1 = 0, uint arg2 = 0, uint arg3 = 0)
		{
			base.initInteractiveElementSkill(arg1, arg2);
			this.nameId = arg3;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.nameId = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_InteractiveElementNamedSkill(arg1);
		}
		
		public void serializeAs_InteractiveElementNamedSkill(BigEndianWriter arg1)
		{
			base.serializeAs_InteractiveElementSkill(arg1);
			if ( this.nameId < 0 )
			{
				throw new Exception("Forbidden value (" + this.nameId + ") on element nameId.");
			}
			arg1.WriteInt((int)this.nameId);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_InteractiveElementNamedSkill(arg1);
		}
		
		public void deserializeAs_InteractiveElementNamedSkill(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.nameId = (uint)arg1.ReadInt();
			if ( this.nameId < 0 )
			{
				throw new Exception("Forbidden value (" + this.nameId + ") on element of InteractiveElementNamedSkill.nameId.");
			}
		}
		
	}
}
