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
	
	public class AdditionalTaxCollectorInformations : Object
	{
		public const uint protocolId = 165;
		public String CollectorCallerName = "";
		public uint date = 0;
		
		public AdditionalTaxCollectorInformations()
		{
		}
		
		public AdditionalTaxCollectorInformations(String arg1, uint arg2)
			: this()
		{
			initAdditionalTaxCollectorInformations(arg1, arg2);
		}
		
		public virtual uint getTypeId()
		{
			return 165;
		}
		
		public AdditionalTaxCollectorInformations initAdditionalTaxCollectorInformations(String arg1 = "", uint arg2 = 0)
		{
			this.CollectorCallerName = arg1;
			this.date = arg2;
			return this;
		}
		
		public virtual void reset()
		{
			this.CollectorCallerName = "";
			this.date = 0;
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_AdditionalTaxCollectorInformations(arg1);
		}
		
		public void serializeAs_AdditionalTaxCollectorInformations(BigEndianWriter arg1)
		{
			arg1.WriteUTF((string)this.CollectorCallerName);
			if ( this.date < 0 )
			{
				throw new Exception("Forbidden value (" + this.date + ") on element date.");
			}
			arg1.WriteInt((int)this.date);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_AdditionalTaxCollectorInformations(arg1);
		}
		
		public void deserializeAs_AdditionalTaxCollectorInformations(BigEndianReader arg1)
		{
			this.CollectorCallerName = (String)arg1.ReadUTF();
			this.date = (uint)arg1.ReadInt();
			if ( this.date < 0 )
			{
				throw new Exception("Forbidden value (" + this.date + ") on element of AdditionalTaxCollectorInformations.date.");
			}
		}
		
	}
}
