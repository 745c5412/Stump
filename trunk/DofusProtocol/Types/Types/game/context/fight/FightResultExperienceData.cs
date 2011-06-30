// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'FightResultExperienceData.xml' the '30/06/2011 11:40:22'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
	public class FightResultExperienceData : FightResultAdditionalData
	{
		public const uint Id = 192;
		public override short TypeId
		{
			get
			{
				return 192;
			}
		}
		
		public bool showExperience;
		public bool showExperienceLevelFloor;
		public bool showExperienceNextLevelFloor;
		public bool showExperienceFightDelta;
		public bool showExperienceForGuild;
		public bool showExperienceForMount;
		public bool isIncarnationExperience;
		public double experience;
		public double experienceLevelFloor;
		public double experienceNextLevelFloor;
		public int experienceFightDelta;
		public int experienceForGuild;
		public int experienceForMount;
		
		public FightResultExperienceData()
		{
		}
		
		public FightResultExperienceData(bool showExperience, bool showExperienceLevelFloor, bool showExperienceNextLevelFloor, bool showExperienceFightDelta, bool showExperienceForGuild, bool showExperienceForMount, bool isIncarnationExperience, double experience, double experienceLevelFloor, double experienceNextLevelFloor, int experienceFightDelta, int experienceForGuild, int experienceForMount)
		{
			this.showExperience = showExperience;
			this.showExperienceLevelFloor = showExperienceLevelFloor;
			this.showExperienceNextLevelFloor = showExperienceNextLevelFloor;
			this.showExperienceFightDelta = showExperienceFightDelta;
			this.showExperienceForGuild = showExperienceForGuild;
			this.showExperienceForMount = showExperienceForMount;
			this.isIncarnationExperience = isIncarnationExperience;
			this.experience = experience;
			this.experienceLevelFloor = experienceLevelFloor;
			this.experienceNextLevelFloor = experienceNextLevelFloor;
			this.experienceFightDelta = experienceFightDelta;
			this.experienceForGuild = experienceForGuild;
			this.experienceForMount = experienceForMount;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			base.Serialize(writer);
			byte flag1 = 0;
			flag1 = BooleanByteWrapper.SetFlag(flag1, 0, showExperience);
			flag1 = BooleanByteWrapper.SetFlag(flag1, 1, showExperienceLevelFloor);
			flag1 = BooleanByteWrapper.SetFlag(flag1, 2, showExperienceNextLevelFloor);
			flag1 = BooleanByteWrapper.SetFlag(flag1, 3, showExperienceFightDelta);
			flag1 = BooleanByteWrapper.SetFlag(flag1, 4, showExperienceForGuild);
			flag1 = BooleanByteWrapper.SetFlag(flag1, 5, showExperienceForMount);
			flag1 = BooleanByteWrapper.SetFlag(flag1, 6, isIncarnationExperience);
			writer.WriteByte(flag1);
			writer.WriteDouble(experience);
			writer.WriteByte(flag1);
			writer.WriteDouble(experienceLevelFloor);
			writer.WriteByte(flag1);
			writer.WriteDouble(experienceNextLevelFloor);
			writer.WriteByte(flag1);
			writer.WriteInt(experienceFightDelta);
			writer.WriteByte(flag1);
			writer.WriteInt(experienceForGuild);
			writer.WriteByte(flag1);
			writer.WriteInt(experienceForMount);
			writer.WriteByte(flag1);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			base.Deserialize(reader);
			byte flag1 = reader.ReadByte();
			showExperience = BooleanByteWrapper.GetFlag(flag1, 0);
			showExperienceLevelFloor = BooleanByteWrapper.GetFlag(flag1, 1);
			showExperienceNextLevelFloor = BooleanByteWrapper.GetFlag(flag1, 2);
			showExperienceFightDelta = BooleanByteWrapper.GetFlag(flag1, 3);
			showExperienceForGuild = BooleanByteWrapper.GetFlag(flag1, 4);
			showExperienceForMount = BooleanByteWrapper.GetFlag(flag1, 5);
			isIncarnationExperience = BooleanByteWrapper.GetFlag(flag1, 6);
			experience = reader.ReadDouble();
			if ( experience < 0 )
			{
				throw new Exception("Forbidden value on experience = " + experience + ", it doesn't respect the following condition : experience < 0");
			}
			experienceLevelFloor = reader.ReadDouble();
			if ( experienceLevelFloor < 0 )
			{
				throw new Exception("Forbidden value on experienceLevelFloor = " + experienceLevelFloor + ", it doesn't respect the following condition : experienceLevelFloor < 0");
			}
			experienceNextLevelFloor = reader.ReadDouble();
			if ( experienceNextLevelFloor < 0 )
			{
				throw new Exception("Forbidden value on experienceNextLevelFloor = " + experienceNextLevelFloor + ", it doesn't respect the following condition : experienceNextLevelFloor < 0");
			}
			experienceFightDelta = reader.ReadInt();
			experienceForGuild = reader.ReadInt();
			if ( experienceForGuild < 0 )
			{
				throw new Exception("Forbidden value on experienceForGuild = " + experienceForGuild + ", it doesn't respect the following condition : experienceForGuild < 0");
			}
			experienceForMount = reader.ReadInt();
			if ( experienceForMount < 0 )
			{
				throw new Exception("Forbidden value on experienceForMount = " + experienceForMount + ", it doesn't respect the following condition : experienceForMount < 0");
			}
		}
	}
}
