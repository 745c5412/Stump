// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'SkillActionDescriptionTimed.xml' the '14/06/2011 11:32:50'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
	public class SkillActionDescriptionTimed : SkillActionDescription
	{
		public const uint Id = 103;
		public short TypeId
		{
			get
			{
				return 103;
			}
		}
		
		public byte time;
		
		public SkillActionDescriptionTimed()
		{
		}
		
		public SkillActionDescriptionTimed(short skillId, byte time)
			 : base(skillId)
		{
			this.time = time;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			base.Serialize(writer);
			writer.WriteByte(time);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			base.Deserialize(reader);
			time = reader.ReadByte();
			if ( time < 0 || time > 255 )
			{
				throw new Exception("Forbidden value on time = " + time + ", it doesn't respect the following condition : time < 0 || time > 255");
			}
		}
	}
}
