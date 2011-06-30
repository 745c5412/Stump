// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'ProtectedEntityWaitingForHelpInfo.xml' the '30/06/2011 11:40:24'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
	public class ProtectedEntityWaitingForHelpInfo
	{
		public const uint Id = 186;
		public virtual short TypeId
		{
			get
			{
				return 186;
			}
		}
		
		public int timeLeftBeforeFight;
		public int waitTimeForPlacement;
		public byte nbPositionForDefensors;
		
		public ProtectedEntityWaitingForHelpInfo()
		{
		}
		
		public ProtectedEntityWaitingForHelpInfo(int timeLeftBeforeFight, int waitTimeForPlacement, byte nbPositionForDefensors)
		{
			this.timeLeftBeforeFight = timeLeftBeforeFight;
			this.waitTimeForPlacement = waitTimeForPlacement;
			this.nbPositionForDefensors = nbPositionForDefensors;
		}
		
		public virtual void Serialize(IDataWriter writer)
		{
			writer.WriteInt(timeLeftBeforeFight);
			writer.WriteInt(waitTimeForPlacement);
			writer.WriteByte(nbPositionForDefensors);
		}
		
		public virtual void Deserialize(IDataReader reader)
		{
			timeLeftBeforeFight = reader.ReadInt();
			waitTimeForPlacement = reader.ReadInt();
			nbPositionForDefensors = reader.ReadByte();
			if ( nbPositionForDefensors < 0 )
			{
				throw new Exception("Forbidden value on nbPositionForDefensors = " + nbPositionForDefensors + ", it doesn't respect the following condition : nbPositionForDefensors < 0");
			}
		}
	}
}
