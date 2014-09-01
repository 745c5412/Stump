

// Generated on 09/01/2014 15:52:13
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class StartupActionsObjetAttributionMessage : Message
    {
        public const uint Id = 1303;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int actionId;
        public int characterId;
        
        public StartupActionsObjetAttributionMessage()
        {
        }
        
        public StartupActionsObjetAttributionMessage(int actionId, int characterId)
        {
            this.actionId = actionId;
            this.characterId = characterId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(actionId);
            writer.WriteInt(characterId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            actionId = reader.ReadInt();
            if (actionId < 0)
                throw new Exception("Forbidden value on actionId = " + actionId + ", it doesn't respect the following condition : actionId < 0");
            characterId = reader.ReadInt();
            if (characterId < 0)
                throw new Exception("Forbidden value on characterId = " + characterId + ", it doesn't respect the following condition : characterId < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(int) + sizeof(int);
        }
        
    }
    
}