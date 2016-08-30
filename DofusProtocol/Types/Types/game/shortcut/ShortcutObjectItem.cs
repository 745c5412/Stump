// Generated on 03/02/2014 20:43:03
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class ShortcutObjectItem : ShortcutObject
    {
        public const short Id = 371;

        public override short TypeId
        {
            get { return Id; }
        }

        public int itemUID;
        public int itemGID;

        public ShortcutObjectItem()
        {
        }

        public ShortcutObjectItem(int slot, int itemUID, int itemGID)
         : base(slot)
        {
            this.itemUID = itemUID;
            this.itemGID = itemGID;
        }

        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(itemUID);
            writer.WriteInt(itemGID);
        }

        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            itemUID = reader.ReadInt();
            itemGID = reader.ReadInt();
        }

        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(int) + sizeof(int);
        }
    }
}