// Generated on 03/02/2014 20:43:02
using Stump.Core.IO;
using System;

namespace Stump.DofusProtocol.Types
{
    public class ProtectedEntityWaitingForHelpInfo
    {
        public const short Id = 186;

        public virtual short TypeId
        {
            get { return Id; }
        }

        public int timeLeftBeforeFight;
        public int waitTimeForPlacement;
        public sbyte nbPositionForDefensors;

        public ProtectedEntityWaitingForHelpInfo()
        {
        }

        public ProtectedEntityWaitingForHelpInfo(int timeLeftBeforeFight, int waitTimeForPlacement, sbyte nbPositionForDefensors)
        {
            this.timeLeftBeforeFight = timeLeftBeforeFight;
            this.waitTimeForPlacement = waitTimeForPlacement;
            this.nbPositionForDefensors = nbPositionForDefensors;
        }

        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteInt(timeLeftBeforeFight);
            writer.WriteInt(waitTimeForPlacement);
            writer.WriteSByte(nbPositionForDefensors);
        }

        public virtual void Deserialize(IDataReader reader)
        {
            timeLeftBeforeFight = reader.ReadInt();
            waitTimeForPlacement = reader.ReadInt();
            nbPositionForDefensors = reader.ReadSByte();
            if (nbPositionForDefensors < 0)
                throw new Exception("Forbidden value on nbPositionForDefensors = " + nbPositionForDefensors + ", it doesn't respect the following condition : nbPositionForDefensors < 0");
        }

        public virtual int GetSerializationSize()
        {
            return sizeof(int) + sizeof(int) + sizeof(sbyte);
        }
    }
}