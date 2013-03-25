
// Generated on 03/25/2013 19:24:31
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class ShortcutSpell : Shortcut
    {
        public const short Id = 368;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public short spellId;
        
        public ShortcutSpell()
        {
        }
        
        public ShortcutSpell(int slot, short spellId)
         : base(slot)
        {
            this.spellId = spellId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(spellId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            spellId = reader.ReadShort();
            if (spellId < 0)
                throw new Exception("Forbidden value on spellId = " + spellId + ", it doesn't respect the following condition : spellId < 0");
        }
        
    }
    
}