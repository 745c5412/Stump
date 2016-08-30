// Generated on 03/02/2014 20:43:02
using Stump.Core.IO;
using System;

namespace Stump.DofusProtocol.Types
{
    public class PaddockBuyableInformations : PaddockInformations
    {
        public const short Id = 130;

        public override short TypeId
        {
            get { return Id; }
        }

        public int price;
        public bool locked;

        public PaddockBuyableInformations()
        {
        }

        public PaddockBuyableInformations(short maxOutdoorMount, short maxItems, int price, bool locked)
         : base(maxOutdoorMount, maxItems)
        {
            this.price = price;
            this.locked = locked;
        }

        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(price);
            writer.WriteBoolean(locked);
        }

        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            price = reader.ReadInt();
            if (price < 0)
                throw new Exception("Forbidden value on price = " + price + ", it doesn't respect the following condition : price < 0");
            locked = reader.ReadBoolean();
        }

        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(int) + sizeof(bool);
        }
    }
}