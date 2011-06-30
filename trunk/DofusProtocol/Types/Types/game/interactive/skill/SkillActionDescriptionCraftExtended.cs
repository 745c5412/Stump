// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'SkillActionDescriptionCraftExtended.xml' the '30/06/2011 11:40:24'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
	public class SkillActionDescriptionCraftExtended : SkillActionDescriptionCraft
	{
		public const uint Id = 104;
		public override short TypeId
		{
			get
			{
				return 104;
			}
		}
		
		public byte thresholdSlots;
		public byte optimumProbability;
		
		public SkillActionDescriptionCraftExtended()
		{
		}
		
		public SkillActionDescriptionCraftExtended(short skillId, byte maxSlots, byte probability, byte thresholdSlots, byte optimumProbability)
			 : base(skillId, maxSlots, probability)
		{
			this.thresholdSlots = thresholdSlots;
			this.optimumProbability = optimumProbability;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			base.Serialize(writer);
			writer.WriteByte(thresholdSlots);
			writer.WriteByte(optimumProbability);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			base.Deserialize(reader);
			thresholdSlots = reader.ReadByte();
			if ( thresholdSlots < 0 )
			{
				throw new Exception("Forbidden value on thresholdSlots = " + thresholdSlots + ", it doesn't respect the following condition : thresholdSlots < 0");
			}
			optimumProbability = reader.ReadByte();
			if ( optimumProbability < 0 )
			{
				throw new Exception("Forbidden value on optimumProbability = " + optimumProbability + ", it doesn't respect the following condition : optimumProbability < 0");
			}
		}
	}
}
