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
	
	public class MountInformationsForPaddock : Object
	{
		public const uint protocolId = 184;
		public int modelId = 0;
		public String name = "";
		public String ownerName = "";
		
		public MountInformationsForPaddock()
		{
		}
		
		public MountInformationsForPaddock(int arg1, String arg2, String arg3)
			: this()
		{
			initMountInformationsForPaddock(arg1, arg2, arg3);
		}
		
		public virtual uint getTypeId()
		{
			return 184;
		}
		
		public MountInformationsForPaddock initMountInformationsForPaddock(int arg1 = 0, String arg2 = "", String arg3 = "")
		{
			this.modelId = arg1;
			this.name = arg2;
			this.ownerName = arg3;
			return this;
		}
		
		public virtual void reset()
		{
			this.modelId = 0;
			this.name = "";
			this.ownerName = "";
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_MountInformationsForPaddock(arg1);
		}
		
		public void serializeAs_MountInformationsForPaddock(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.modelId);
			arg1.WriteUTF((string)this.name);
			arg1.WriteUTF((string)this.ownerName);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_MountInformationsForPaddock(arg1);
		}
		
		public void deserializeAs_MountInformationsForPaddock(BigEndianReader arg1)
		{
			this.modelId = (int)arg1.ReadInt();
			this.name = (String)arg1.ReadUTF();
			this.ownerName = (String)arg1.ReadUTF();
		}
		
	}
}
