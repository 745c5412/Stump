// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'QuestStepInfoRequestMessage.xml' the '30/06/2011 11:40:15'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class QuestStepInfoRequestMessage : Message
	{
		public const uint Id = 5622;
		public override uint MessageId
		{
			get
			{
				return 5622;
			}
		}
		
		public ushort questId;
		
		public QuestStepInfoRequestMessage()
		{
		}
		
		public QuestStepInfoRequestMessage(ushort questId)
		{
			this.questId = questId;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteUShort(questId);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			questId = reader.ReadUShort();
			if ( questId < 0 || questId > 65535 )
			{
				throw new Exception("Forbidden value on questId = " + questId + ", it doesn't respect the following condition : questId < 0 || questId > 65535");
			}
		}
	}
}
