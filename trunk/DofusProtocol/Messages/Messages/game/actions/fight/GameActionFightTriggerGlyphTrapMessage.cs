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
	
	public class GameActionFightTriggerGlyphTrapMessage : AbstractGameActionMessage
	{
		public const uint protocolId = 5741;
		internal Boolean _isInitialized = false;
		public int markId = 0;
		public int triggeringCharacterId = 0;
		public uint triggeredSpellId = 0;
		
		public GameActionFightTriggerGlyphTrapMessage()
		{
		}
		
		public GameActionFightTriggerGlyphTrapMessage(uint arg1, int arg2, int arg3, int arg4, uint arg5)
			: this()
		{
			initGameActionFightTriggerGlyphTrapMessage(arg1, arg2, arg3, arg4, arg5);
		}
		
		public override uint getMessageId()
		{
			return 5741;
		}
		
		public GameActionFightTriggerGlyphTrapMessage initGameActionFightTriggerGlyphTrapMessage(uint arg1 = 0, int arg2 = 0, int arg3 = 0, int arg4 = 0, uint arg5 = 0)
		{
			base.initAbstractGameActionMessage(arg1, arg2);
			this.markId = arg3;
			this.triggeringCharacterId = arg4;
			this.triggeredSpellId = arg5;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.markId = 0;
			this.triggeringCharacterId = 0;
			this.triggeredSpellId = 0;
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
			this.serializeAs_GameActionFightTriggerGlyphTrapMessage(arg1);
		}
		
		public void serializeAs_GameActionFightTriggerGlyphTrapMessage(BigEndianWriter arg1)
		{
			base.serializeAs_AbstractGameActionMessage(arg1);
			arg1.WriteShort((short)this.markId);
			arg1.WriteInt((int)this.triggeringCharacterId);
			if ( this.triggeredSpellId < 0 )
			{
				throw new Exception("Forbidden value (" + this.triggeredSpellId + ") on element triggeredSpellId.");
			}
			arg1.WriteShort((short)this.triggeredSpellId);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameActionFightTriggerGlyphTrapMessage(arg1);
		}
		
		public void deserializeAs_GameActionFightTriggerGlyphTrapMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.markId = (int)arg1.ReadShort();
			this.triggeringCharacterId = (int)arg1.ReadInt();
			this.triggeredSpellId = (uint)arg1.ReadShort();
			if ( this.triggeredSpellId < 0 )
			{
				throw new Exception("Forbidden value (" + this.triggeredSpellId + ") on element of GameActionFightTriggerGlyphTrapMessage.triggeredSpellId.");
			}
		}
		
	}
}
