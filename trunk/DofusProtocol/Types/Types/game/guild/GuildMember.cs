// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'GuildMember.xml' the '14/06/2011 11:32:49'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
	public class GuildMember : CharacterMinimalInformations
	{
		public const uint Id = 88;
		public short TypeId
		{
			get
			{
				return 88;
			}
		}
		
		public byte breed;
		public bool sex;
		public short rank;
		public double givenExperience;
		public byte experienceGivenPercent;
		public uint rights;
		public byte connected;
		public byte alignmentSide;
		public ushort hoursSinceLastConnection;
		public byte moodSmileyId;
		
		public GuildMember()
		{
		}
		
		public GuildMember(int id, byte level, string name, byte breed, bool sex, short rank, double givenExperience, byte experienceGivenPercent, uint rights, byte connected, byte alignmentSide, ushort hoursSinceLastConnection, byte moodSmileyId)
			 : base(id, level, name)
		{
			this.breed = breed;
			this.sex = sex;
			this.rank = rank;
			this.givenExperience = givenExperience;
			this.experienceGivenPercent = experienceGivenPercent;
			this.rights = rights;
			this.connected = connected;
			this.alignmentSide = alignmentSide;
			this.hoursSinceLastConnection = hoursSinceLastConnection;
			this.moodSmileyId = moodSmileyId;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			base.Serialize(writer);
			writer.WriteByte(breed);
			writer.WriteBoolean(sex);
			writer.WriteShort(rank);
			writer.WriteDouble(givenExperience);
			writer.WriteByte(experienceGivenPercent);
			writer.WriteUInt(rights);
			writer.WriteByte(connected);
			writer.WriteByte(alignmentSide);
			writer.WriteUShort(hoursSinceLastConnection);
			writer.WriteByte(moodSmileyId);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			base.Deserialize(reader);
			breed = reader.ReadByte();
			sex = reader.ReadBoolean();
			rank = reader.ReadShort();
			if ( rank < 0 )
			{
				throw new Exception("Forbidden value on rank = " + rank + ", it doesn't respect the following condition : rank < 0");
			}
			givenExperience = reader.ReadDouble();
			if ( givenExperience < 0 )
			{
				throw new Exception("Forbidden value on givenExperience = " + givenExperience + ", it doesn't respect the following condition : givenExperience < 0");
			}
			experienceGivenPercent = reader.ReadByte();
			if ( experienceGivenPercent < 0 || experienceGivenPercent > 100 )
			{
				throw new Exception("Forbidden value on experienceGivenPercent = " + experienceGivenPercent + ", it doesn't respect the following condition : experienceGivenPercent < 0 || experienceGivenPercent > 100");
			}
			rights = reader.ReadUInt();
			if ( rights < 0 || rights > 4294967295 )
			{
				throw new Exception("Forbidden value on rights = " + rights + ", it doesn't respect the following condition : rights < 0 || rights > 4294967295");
			}
			connected = reader.ReadByte();
			if ( connected < 0 )
			{
				throw new Exception("Forbidden value on connected = " + connected + ", it doesn't respect the following condition : connected < 0");
			}
			alignmentSide = reader.ReadByte();
			hoursSinceLastConnection = reader.ReadUShort();
			if ( hoursSinceLastConnection < 0 || hoursSinceLastConnection > 65535 )
			{
				throw new Exception("Forbidden value on hoursSinceLastConnection = " + hoursSinceLastConnection + ", it doesn't respect the following condition : hoursSinceLastConnection < 0 || hoursSinceLastConnection > 65535");
			}
			moodSmileyId = reader.ReadByte();
		}
	}
}
