// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'AchievementListMessage.xml' the '15/06/2011 01:38:39'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class AchievementListMessage : Message
	{
		public const uint Id = 6205;
		public override uint MessageId
		{
			get
			{
				return 6205;
			}
		}
		
		public Types.Achievement[] startedAchievements;
		public short[] finishedAchievementsIds;
		
		public AchievementListMessage()
		{
		}
		
		public AchievementListMessage(Types.Achievement[] startedAchievements, short[] finishedAchievementsIds)
		{
			this.startedAchievements = startedAchievements;
			this.finishedAchievementsIds = finishedAchievementsIds;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteUShort((ushort)startedAchievements.Length);
			for (int i = 0; i < startedAchievements.Length; i++)
			{
				writer.WriteShort(startedAchievements[i].TypeId);
				startedAchievements[i].Serialize(writer);
			}
			writer.WriteUShort((ushort)finishedAchievementsIds.Length);
			for (int i = 0; i < finishedAchievementsIds.Length; i++)
			{
				writer.WriteShort(finishedAchievementsIds[i]);
			}
		}
		
		public override void Deserialize(IDataReader reader)
		{
			int limit = reader.ReadUShort();
			startedAchievements = new Types.Achievement[limit];
			for (int i = 0; i < limit; i++)
			{
				startedAchievements[i] = Types.ProtocolTypeManager.GetInstance<Types.Achievement>(reader.ReadShort());
				startedAchievements[i].Deserialize(reader);
			}
			limit = reader.ReadUShort();
			finishedAchievementsIds = new short[limit];
			for (int i = 0; i < limit; i++)
			{
				finishedAchievementsIds[i] = reader.ReadShort();
			}
		}
	}
}
