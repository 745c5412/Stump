// Generated on 03/02/2014 20:43:03
using Stump.Core.IO;
using System;

namespace Stump.DofusProtocol.Types
{
    public class Shortcut
    {
        public const short Id = 369;

        public virtual short TypeId
        {
            get { return Id; }
        }

        public int slot;

        public Shortcut()
        {
        }

        public Shortcut(int slot)
        {
            this.slot = slot;
        }

        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteInt(slot);
        }

        public virtual void Deserialize(IDataReader reader)
        {
            slot = reader.ReadInt();
            if (slot < 0 || slot > 99)
                throw new Exception("Forbidden value on slot = " + slot + ", it doesn't respect the following condition : slot < 0 || slot > 99");
        }

        public virtual int GetSerializationSize()
        {
            return sizeof(int);
        }
    }
}