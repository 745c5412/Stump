// Generated on 03/02/2014 20:43:00
using Stump.Core.IO;
using System.Text;

namespace Stump.DofusProtocol.Types
{
    public class GameRolePlayNamedActorInformations : GameRolePlayActorInformations
    {
        public const short Id = 154;

        public override short TypeId
        {
            get { return Id; }
        }

        public string name;

        public GameRolePlayNamedActorInformations()
        {
        }

        public GameRolePlayNamedActorInformations(int contextualId, Types.EntityLook look, Types.EntityDispositionInformations disposition, string name)
         : base(contextualId, look, disposition)
        {
            this.name = name;
        }

        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUTF(name);
        }

        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            name = reader.ReadUTF();
        }

        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(short) + Encoding.UTF8.GetByteCount(name);
        }
    }
}