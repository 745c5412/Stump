// Generated on 03/02/2014 20:42:59
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class FightTeamMemberInformations
    {
        public const short Id = 44;

        public virtual short TypeId
        {
            get { return Id; }
        }

        public int id;

        public FightTeamMemberInformations()
        {
        }

        public FightTeamMemberInformations(int id)
        {
            this.id = id;
        }

        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteInt(id);
        }

        public virtual void Deserialize(IDataReader reader)
        {
            id = reader.ReadInt();
        }

        public virtual int GetSerializationSize()
        {
            return sizeof(int);
        }
    }
}