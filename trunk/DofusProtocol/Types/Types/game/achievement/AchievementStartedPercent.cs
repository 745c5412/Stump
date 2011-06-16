// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'AchievementStartedPercent.xml' the '14/06/2011 11:32:44'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
	public class AchievementStartedPercent : Achievement
	{
		public const uint Id = 362;
		public short TypeId
		{
			get
			{
				return 362;
			}
		}
		
		public byte completionPercent;
		
		public AchievementStartedPercent()
		{
		}
		
		public AchievementStartedPercent(short id, byte completionPercent)
			 : base(id)
		{
			this.completionPercent = completionPercent;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			base.Serialize(writer);
			writer.WriteByte(completionPercent);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			base.Deserialize(reader);
			completionPercent = reader.ReadByte();
			if ( completionPercent < 0 )
			{
				throw new Exception("Forbidden value on completionPercent = " + completionPercent + ", it doesn't respect the following condition : completionPercent < 0");
			}
		}
	}
}
