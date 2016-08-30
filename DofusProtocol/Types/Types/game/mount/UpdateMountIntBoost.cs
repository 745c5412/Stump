// Generated on 03/02/2014 20:43:02
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class UpdateMountIntBoost : UpdateMountBoost
    {
        public const short Id = 357;

        public override short TypeId
        {
            get { return Id; }
        }

        public int value;

        public UpdateMountIntBoost()
        {
        }

        public UpdateMountIntBoost(sbyte type, int value)
         : base(type)
        {
            this.value = value;
        }

        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(value);
        }

        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            value = reader.ReadInt();
        }

        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(int);
        }
    }
}