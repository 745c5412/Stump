

// Generated on 07/26/2013 22:51:10
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class CharacterSpellModification
    {
        public const short Id = 215;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public sbyte modificationType;
        public short spellId;
        public Types.CharacterBaseCharacteristic value;
        
        public CharacterSpellModification()
        {
        }
        
        public CharacterSpellModification(sbyte modificationType, short spellId, Types.CharacterBaseCharacteristic value)
        {
            this.modificationType = modificationType;
            this.spellId = spellId;
            this.value = value;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(modificationType);
            writer.WriteShort(spellId);
            value.Serialize(writer);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            modificationType = reader.ReadSByte();
            if (modificationType < 0)
                throw new Exception("Forbidden value on modificationType = " + modificationType + ", it doesn't respect the following condition : modificationType < 0");
            spellId = reader.ReadShort();
            if (spellId < 0)
                throw new Exception("Forbidden value on spellId = " + spellId + ", it doesn't respect the following condition : spellId < 0");
            value = new Types.CharacterBaseCharacteristic();
            value.Deserialize(reader);
        }
        
        public virtual int GetSerializationSize()
        {
            return sizeof(sbyte) + sizeof(short) + value.GetSerializationSize();
        }
        
    }
    
}