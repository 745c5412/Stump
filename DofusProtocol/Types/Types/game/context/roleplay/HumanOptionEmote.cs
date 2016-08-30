// Generated on 03/02/2014 20:43:00
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class HumanOptionEmote : HumanOption
    {
        public const short Id = 407;

        public override short TypeId
        {
            get { return Id; }
        }

        public sbyte emoteId;
        public double emoteStartTime;

        public HumanOptionEmote()
        {
        }

        public HumanOptionEmote(sbyte emoteId, double emoteStartTime)
        {
            this.emoteId = emoteId;
            this.emoteStartTime = emoteStartTime;
        }

        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteSByte(emoteId);
            writer.WriteDouble(emoteStartTime);
        }

        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            emoteId = reader.ReadSByte();
            emoteStartTime = reader.ReadDouble();
        }

        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(sbyte) + sizeof(double);
        }
    }
}