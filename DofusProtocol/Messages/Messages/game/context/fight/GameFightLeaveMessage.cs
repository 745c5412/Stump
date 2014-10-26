

// Generated on 10/26/2014 23:29:22
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameFightLeaveMessage : Message
    {
        public const uint Id = 721;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int charId;
        
        public GameFightLeaveMessage()
        {
        }
        
        public GameFightLeaveMessage(int charId)
        {
            this.charId = charId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(charId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            charId = reader.ReadInt();
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(int);
        }
        
    }
    
}