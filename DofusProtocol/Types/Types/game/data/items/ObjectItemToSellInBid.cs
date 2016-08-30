// Generated on 03/02/2014 20:43:01
using Stump.Core.IO;
using System;
using System.Collections.Generic;

namespace Stump.DofusProtocol.Types
{
    public class ObjectItemToSellInBid : ObjectItemToSell
    {
        public const short Id = 164;

        public override short TypeId
        {
            get { return Id; }
        }

        public short unsoldDelay;

        public ObjectItemToSellInBid()
        {
        }

        public ObjectItemToSellInBid(short objectGID, short powerRate, bool overMax, IEnumerable<Types.ObjectEffect> effects, int objectUID, int quantity, int objectPrice, short unsoldDelay)
         : base(objectGID, powerRate, overMax, effects, objectUID, quantity, objectPrice)
        {
            this.unsoldDelay = unsoldDelay;
        }

        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(unsoldDelay);
        }

        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            unsoldDelay = reader.ReadShort();
            if (unsoldDelay < 0)
                throw new Exception("Forbidden value on unsoldDelay = " + unsoldDelay + ", it doesn't respect the following condition : unsoldDelay < 0");
        }

        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(short);
        }
    }
}