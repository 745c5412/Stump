// Generated on 03/02/2014 20:43:00
using Stump.Core.IO;
using System;

namespace Stump.DofusProtocol.Types
{
    public class QuestActiveInformations
    {
        public const short Id = 381;

        public virtual short TypeId
        {
            get { return Id; }
        }

        public short questId;

        public QuestActiveInformations()
        {
        }

        public QuestActiveInformations(short questId)
        {
            this.questId = questId;
        }

        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteShort(questId);
        }

        public virtual void Deserialize(IDataReader reader)
        {
            questId = reader.ReadShort();
            if (questId < 0)
                throw new Exception("Forbidden value on questId = " + questId + ", it doesn't respect the following condition : questId < 0");
        }

        public virtual int GetSerializationSize()
        {
            return sizeof(short);
        }
    }
}