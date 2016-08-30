// Generated on 03/02/2014 20:42:59
using Stump.Core.IO;
using System;
using System.Text;

namespace Stump.DofusProtocol.Types
{
    public class FightTeamMemberCharacterInformations : FightTeamMemberInformations
    {
        public const short Id = 13;

        public override short TypeId
        {
            get { return Id; }
        }

        public string name;
        public short level;

        public FightTeamMemberCharacterInformations()
        {
        }

        public FightTeamMemberCharacterInformations(int id, string name, short level)
         : base(id)
        {
            this.name = name;
            this.level = level;
        }

        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUTF(name);
            writer.WriteShort(level);
        }

        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            name = reader.ReadUTF();
            level = reader.ReadShort();
            if (level < 0)
                throw new Exception("Forbidden value on level = " + level + ", it doesn't respect the following condition : level < 0");
        }

        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(short) + Encoding.UTF8.GetByteCount(name) + sizeof(short);
        }
    }
}