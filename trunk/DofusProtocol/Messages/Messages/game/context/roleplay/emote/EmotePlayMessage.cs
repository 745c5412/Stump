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
	
	public class EmotePlayMessage : EmotePlayAbstractMessage
	{
		public const uint protocolId = 5683;
		internal Boolean _isInitialized = false;
		public int actorId = 0;
		public int accountId = 0;
		
		public EmotePlayMessage()
		{
		}
		
		public EmotePlayMessage(uint arg1, uint arg2, int arg3, int arg4)
			: this()
		{
			initEmotePlayMessage(arg1, arg2, arg3, arg4);
		}
		
		public override uint getMessageId()
		{
			return 5683;
		}
		
		public EmotePlayMessage initEmotePlayMessage(uint arg1 = 0, uint arg2 = 0, int arg3 = 0, int arg4 = 0)
		{
			base.initEmotePlayAbstractMessage(arg1, arg2);
			this.actorId = arg3;
			this.accountId = arg4;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.actorId = 0;
			this.accountId = 0;
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
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_EmotePlayMessage(arg1);
		}
		
		public void serializeAs_EmotePlayMessage(BigEndianWriter arg1)
		{
			base.serializeAs_EmotePlayAbstractMessage(arg1);
			arg1.WriteInt((int)this.actorId);
			arg1.WriteInt((int)this.accountId);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_EmotePlayMessage(arg1);
		}
		
		public void deserializeAs_EmotePlayMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.actorId = (int)arg1.ReadInt();
			this.accountId = (int)arg1.ReadInt();
		}
		
	}
}
