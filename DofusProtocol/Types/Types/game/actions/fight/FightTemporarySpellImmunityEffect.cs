// Generated on 03/02/2014 20:42:58
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class FightTemporarySpellImmunityEffect : AbstractFightDispellableEffect
    {
        public const short Id = 366;

        public override short TypeId
        {
            get { return Id; }
        }

        public int immuneSpellId;

        public FightTemporarySpellImmunityEffect()
        {
        }

        public FightTemporarySpellImmunityEffect(int uid, int targetId, short turnDuration, sbyte dispelable, short spellId, int parentBoostUid, int immuneSpellId)
         : base(uid, targetId, turnDuration, dispelable, spellId, parentBoostUid)
        {
            this.immuneSpellId = immuneSpellId;
        }

        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(immuneSpellId);
        }

        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            immuneSpellId = reader.ReadInt();
        }

        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(int);
        }
    }
}