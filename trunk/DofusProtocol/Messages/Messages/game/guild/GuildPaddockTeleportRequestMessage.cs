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
	
	public class GuildPaddockTeleportRequestMessage : Message
	{
		public const uint protocolId = 5957;
		internal Boolean _isInitialized = false;
		public uint paddockId = 0;
		
		public GuildPaddockTeleportRequestMessage()
		{
		}
		
		public GuildPaddockTeleportRequestMessage(uint arg1)
			: this()
		{
			initGuildPaddockTeleportRequestMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5957;
		}
		
		public GuildPaddockTeleportRequestMessage initGuildPaddockTeleportRequestMessage(uint arg1 = 0)
		{
			this.paddockId = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.paddockId = 0;
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
			this.serializeAs_GuildPaddockTeleportRequestMessage(arg1);
		}
		
		public void serializeAs_GuildPaddockTeleportRequestMessage(BigEndianWriter arg1)
		{
			if ( this.paddockId < 0 )
			{
				throw new Exception("Forbidden value (" + this.paddockId + ") on element paddockId.");
			}
			arg1.WriteInt((int)this.paddockId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GuildPaddockTeleportRequestMessage(arg1);
		}
		
		public void deserializeAs_GuildPaddockTeleportRequestMessage(BigEndianReader arg1)
		{
			this.paddockId = (uint)arg1.ReadInt();
			if ( this.paddockId < 0 )
			{
				throw new Exception("Forbidden value (" + this.paddockId + ") on element of GuildPaddockTeleportRequestMessage.paddockId.");
			}
		}
		
	}
}
