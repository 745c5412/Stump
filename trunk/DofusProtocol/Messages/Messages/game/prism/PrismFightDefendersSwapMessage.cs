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
	
	public class PrismFightDefendersSwapMessage : Message
	{
		public const uint protocolId = 5902;
		internal Boolean _isInitialized = false;
		public double fightId = 0;
		public uint fighterId1 = 0;
		public uint fighterId2 = 0;
		
		public PrismFightDefendersSwapMessage()
		{
		}
		
		public PrismFightDefendersSwapMessage(double arg1, uint arg2, uint arg3)
			: this()
		{
			initPrismFightDefendersSwapMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 5902;
		}
		
		public PrismFightDefendersSwapMessage initPrismFightDefendersSwapMessage(double arg1 = 0, uint arg2 = 0, uint arg3 = 0)
		{
			this.fightId = arg1;
			this.fighterId1 = arg2;
			this.fighterId2 = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.fightId = 0;
			this.fighterId1 = 0;
			this.fighterId2 = 0;
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
			this.serializeAs_PrismFightDefendersSwapMessage(arg1);
		}
		
		public void serializeAs_PrismFightDefendersSwapMessage(BigEndianWriter arg1)
		{
			arg1.WriteDouble(this.fightId);
			if ( this.fighterId1 < 0 )
			{
				throw new Exception("Forbidden value (" + this.fighterId1 + ") on element fighterId1.");
			}
			arg1.WriteInt((int)this.fighterId1);
			if ( this.fighterId2 < 0 )
			{
				throw new Exception("Forbidden value (" + this.fighterId2 + ") on element fighterId2.");
			}
			arg1.WriteInt((int)this.fighterId2);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PrismFightDefendersSwapMessage(arg1);
		}
		
		public void deserializeAs_PrismFightDefendersSwapMessage(BigEndianReader arg1)
		{
			this.fightId = (double)arg1.ReadDouble();
			this.fighterId1 = (uint)arg1.ReadInt();
			if ( this.fighterId1 < 0 )
			{
				throw new Exception("Forbidden value (" + this.fighterId1 + ") on element of PrismFightDefendersSwapMessage.fighterId1.");
			}
			this.fighterId2 = (uint)arg1.ReadInt();
			if ( this.fighterId2 < 0 )
			{
				throw new Exception("Forbidden value (" + this.fighterId2 + ") on element of PrismFightDefendersSwapMessage.fighterId2.");
			}
		}
		
	}
}
