
// Generated on 03/25/2013 19:24:27
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class CharacterMinimalPlusLookInformations : CharacterMinimalInformations
    {
        public const short Id = 163;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public Types.EntityLook entityLook;
        
        public CharacterMinimalPlusLookInformations()
        {
        }
        
        public CharacterMinimalPlusLookInformations(int id, byte level, string name, Types.EntityLook entityLook)
         : base(id, level, name)
        {
            this.entityLook = entityLook;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            entityLook.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            entityLook = new Types.EntityLook();
            entityLook.Deserialize(reader);
        }
        
    }
    
}