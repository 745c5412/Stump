// Generated on 03/02/2014 20:42:59
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class FightResultAdditionalData
    {
        public const short Id = 191;

        public virtual short TypeId
        {
            get { return Id; }
        }

        public FightResultAdditionalData()
        {
        }

        public virtual void Serialize(IDataWriter writer)
        {
        }

        public virtual void Deserialize(IDataReader reader)
        {
        }

        public virtual int GetSerializationSize()
        {
            return 0;
            ;
        }
    }
}