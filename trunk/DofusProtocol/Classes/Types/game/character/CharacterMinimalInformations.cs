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
	
	public class CharacterMinimalInformations : Object
	{
		public const uint protocolId = 110;
		public uint id = 0;
		public uint level = 0;
		public String name = "";
		
		public CharacterMinimalInformations()
		{
		}
		
		public CharacterMinimalInformations(uint arg1, uint arg2, String arg3)
			: this()
		{
			initCharacterMinimalInformations(arg1, arg2, arg3);
		}
		
		public virtual uint getTypeId()
		{
			return 110;
		}
		
		public CharacterMinimalInformations initCharacterMinimalInformations(uint arg1 = 0, uint arg2 = 0, String arg3 = "")
		{
			this.id = arg1;
			this.level = arg2;
			this.name = arg3;
			return this;
		}
		
		public virtual void reset()
		{
			this.id = 0;
			this.level = 0;
			this.name = "";
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_CharacterMinimalInformations(arg1);
		}
		
		public void serializeAs_CharacterMinimalInformations(BigEndianWriter arg1)
		{
			if ( this.id < 0 )
			{
				throw new Exception("Forbidden value (" + this.id + ") on element id.");
			}
			arg1.WriteInt((int)this.id);
			if ( this.level < 1 || this.level > 200 )
			{
				throw new Exception("Forbidden value (" + this.level + ") on element level.");
			}
			arg1.WriteByte((byte)this.level);
			arg1.WriteUTF((string)this.name);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_CharacterMinimalInformations(arg1);
		}
		
		public void deserializeAs_CharacterMinimalInformations(BigEndianReader arg1)
		{
			this.id = (uint)arg1.ReadInt();
			if ( this.id < 0 )
			{
				throw new Exception("Forbidden value (" + this.id + ") on element of CharacterMinimalInformations.id.");
			}
			this.level = (uint)arg1.ReadByte();
			if ( this.level < 1 || this.level > 200 )
			{
				throw new Exception("Forbidden value (" + this.level + ") on element of CharacterMinimalInformations.level.");
			}
			this.name = (String)arg1.ReadUTF();
		}
		
	}
}
