

// Generated on 09/01/2014 15:51:50
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameActionFightDispellSpellMessage : GameActionFightDispellMessage
    {
        public const uint Id = 6176;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int spellId;
        
        public GameActionFightDispellSpellMessage()
        {
        }
        
        public GameActionFightDispellSpellMessage(short actionId, int sourceId, int targetId, int spellId)
         : base(actionId, sourceId, targetId)
        {
            this.spellId = spellId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(spellId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            spellId = reader.ReadInt();
            if (spellId < 0)
                throw new Exception("Forbidden value on spellId = " + spellId + ", it doesn't respect the following condition : spellId < 0");
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(int);
        }
        
    }
    
}