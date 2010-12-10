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
	
	public class BasicPingMessage : Message
	{
		public const uint protocolId = 182;
		internal Boolean _isInitialized = false;
		public Boolean quiet = false;
		
		public BasicPingMessage()
		{
		}
		
		public BasicPingMessage(Boolean arg1)
			: this()
		{
			initBasicPingMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 182;
		}
		
		public BasicPingMessage initBasicPingMessage(Boolean arg1 = false)
		{
			this.quiet = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.quiet = false;
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
			this.serializeAs_BasicPingMessage(arg1);
		}
		
		public void serializeAs_BasicPingMessage(BigEndianWriter arg1)
		{
			arg1.WriteBoolean(this.quiet);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_BasicPingMessage(arg1);
		}
		
		public void deserializeAs_BasicPingMessage(BigEndianReader arg1)
		{
			this.quiet = (Boolean)arg1.ReadBoolean();
		}
		
	}
}
