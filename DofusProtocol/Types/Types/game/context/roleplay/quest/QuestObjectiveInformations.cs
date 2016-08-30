// Generated on 03/02/2014 20:43:01
using Stump.Core.IO;
using System;

namespace Stump.DofusProtocol.Types
{
    public class QuestObjectiveInformations
    {
        public const short Id = 385;

        public virtual short TypeId
        {
            get { return Id; }
        }

        public short objectiveId;
        public bool objectiveStatus;

        public QuestObjectiveInformations()
        {
        }

        public QuestObjectiveInformations(short objectiveId, bool objectiveStatus)
        {
            this.objectiveId = objectiveId;
            this.objectiveStatus = objectiveStatus;
        }

        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteShort(objectiveId);
            writer.WriteBoolean(objectiveStatus);
        }

        public virtual void Deserialize(IDataReader reader)
        {
            objectiveId = reader.ReadShort();
            if (objectiveId < 0)
                throw new Exception("Forbidden value on objectiveId = " + objectiveId + ", it doesn't respect the following condition : objectiveId < 0");
            objectiveStatus = reader.ReadBoolean();
        }

        public virtual int GetSerializationSize()
        {
            return sizeof(short) + sizeof(bool);
        }
    }
}