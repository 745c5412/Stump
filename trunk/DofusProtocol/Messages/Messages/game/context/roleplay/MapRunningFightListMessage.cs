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
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class MapRunningFightListMessage : Message
	{
		public const uint protocolId = 5743;
		internal Boolean _isInitialized = false;
		public List<FightExternalInformations> fights;
		
		public MapRunningFightListMessage()
		{
			this.fights = new List<FightExternalInformations>();
		}
		
		public MapRunningFightListMessage(List<FightExternalInformations> arg1)
			: this()
		{
			initMapRunningFightListMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5743;
		}
		
		public MapRunningFightListMessage initMapRunningFightListMessage(List<FightExternalInformations> arg1)
		{
			this.fights = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.fights = new List<FightExternalInformations>();
			this._isInitialized = false;
		}
		
		public override void pack(BigEndianWriter arg1)
		{
			this.serialize(arg1);
			WritePacket(arg1, this.getMessageId());
		}
		
		public override void unpack(BigEndianReader arg1, uint arg2)
		{
			this.deserialize(arg1);
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_MapRunningFightListMessage(arg1);
		}
		
		public void serializeAs_MapRunningFightListMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.fights.Count);
			var loc1 = 0;
			while ( loc1 < this.fights.Count )
			{
				this.fights[loc1].serializeAs_FightExternalInformations(arg1);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_MapRunningFightListMessage(arg1);
		}
		
		public void deserializeAs_MapRunningFightListMessage(BigEndianReader arg1)
		{
			object loc3 = null;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new FightExternalInformations()) as FightExternalInformations).deserialize(arg1);
				this.fights.Add((FightExternalInformations)loc3);
				++loc2;
			}
		}
		
	}
}
