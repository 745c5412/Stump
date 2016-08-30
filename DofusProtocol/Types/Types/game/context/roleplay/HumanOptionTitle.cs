// Generated on 03/02/2014 20:43:00
using Stump.Core.IO;
using System;
using System.Text;

namespace Stump.DofusProtocol.Types
{
    public class HumanOptionTitle : HumanOption
    {
        public const short Id = 408;

        public override short TypeId
        {
            get { return Id; }
        }

        public short titleId;
        public string titleParam;

        public HumanOptionTitle()
        {
        }

        public HumanOptionTitle(short titleId, string titleParam)
        {
            this.titleId = titleId;
            this.titleParam = titleParam;
        }

        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(titleId);
            writer.WriteUTF(titleParam);
        }

        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            titleId = reader.ReadShort();
            if (titleId < 0)
                throw new Exception("Forbidden value on titleId = " + titleId + ", it doesn't respect the following condition : titleId < 0");
            titleParam = reader.ReadUTF();
        }

        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(short) + sizeof(short) + Encoding.UTF8.GetByteCount(titleParam);
        }
    }
}