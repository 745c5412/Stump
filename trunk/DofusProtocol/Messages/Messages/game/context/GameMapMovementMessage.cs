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
	
	public class GameMapMovementMessage : Message
	{
		public const uint protocolId = 951;
		internal Boolean _isInitialized = false;
		public List<uint> keyMovements;
		public int actorId = 0;
		
		public GameMapMovementMessage()
		{
			this.keyMovements = new List<uint>();
		}
		
		public GameMapMovementMessage(List<uint> arg1, int arg2)
			: this()
		{
			initGameMapMovementMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 951;
		}
		
		public GameMapMovementMessage initGameMapMovementMessage(List<uint> arg1, int arg2 = 0)
		{
			this.keyMovements = arg1;
			this.actorId = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.keyMovements = new List<uint>();
			this.actorId = 0;
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
			this.serializeAs_GameMapMovementMessage(arg1);
		}
		
		public void serializeAs_GameMapMovementMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.keyMovements.Count);
			var loc1 = 0;
			while ( loc1 < this.keyMovements.Count )
			{
				if ( this.keyMovements[loc1] < 0 )
				{
					throw new Exception("Forbidden value (" + this.keyMovements[loc1] + ") on element 1 (starting at 1) of keyMovements.");
				}
				arg1.WriteShort((short)this.keyMovements[loc1]);
				++loc1;
			}
			arg1.WriteInt((int)this.actorId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameMapMovementMessage(arg1);
		}
		
		public void deserializeAs_GameMapMovementMessage(BigEndianReader arg1)
		{
			var loc3 = 0;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				if ( (loc3 = arg1.ReadShort()) < 0 )
				{
					throw new Exception("Forbidden value (" + loc3 + ") on elements of keyMovements.");
				}
				this.keyMovements.Add((uint)loc3);
				++loc2;
			}
			this.actorId = (int)arg1.ReadInt();
		}
		
	}
}
