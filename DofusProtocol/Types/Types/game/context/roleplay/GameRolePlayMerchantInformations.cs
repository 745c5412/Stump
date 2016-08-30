// Generated on 03/02/2014 20:43:00
using Stump.Core.IO;
using System;

namespace Stump.DofusProtocol.Types
{
    public class GameRolePlayMerchantInformations : GameRolePlayNamedActorInformations
    {
        public const short Id = 129;

        public override short TypeId
        {
            get { return Id; }
        }

        public int sellType;

        public GameRolePlayMerchantInformations()
        {
        }

        public GameRolePlayMerchantInformations(int contextualId, Types.EntityLook look, Types.EntityDispositionInformations disposition, string name, int sellType)
         : base(contextualId, look, disposition, name)
        {
            this.sellType = sellType;
        }

        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(sellType);
        }

        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            sellType = reader.ReadInt();
            if (sellType < 0)
                throw new Exception("Forbidden value on sellType = " + sellType + ", it doesn't respect the following condition : sellType < 0");
        }

        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(int);
        }
    }
}