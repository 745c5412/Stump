// Generated on 03/02/2014 20:43:00
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class HumanOption
    {
        public const short Id = 406;

        public virtual short TypeId
        {
            get { return Id; }
        }

        public HumanOption()
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