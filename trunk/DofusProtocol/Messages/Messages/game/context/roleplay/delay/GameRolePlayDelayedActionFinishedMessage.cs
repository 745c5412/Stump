
// Generated on 03/25/2013 19:24:10
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameRolePlayDelayedActionFinishedMessage : Message
    {
        public const uint Id = 6150;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int delayedCharacterId;
        public sbyte delayTypeId;
        
        public GameRolePlayDelayedActionFinishedMessage()
        {
        }
        
        public GameRolePlayDelayedActionFinishedMessage(int delayedCharacterId, sbyte delayTypeId)
        {
            this.delayedCharacterId = delayedCharacterId;
            this.delayTypeId = delayTypeId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(delayedCharacterId);
            writer.WriteSByte(delayTypeId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            delayedCharacterId = reader.ReadInt();
            delayTypeId = reader.ReadSByte();
            if (delayTypeId < 0)
                throw new Exception("Forbidden value on delayTypeId = " + delayTypeId + ", it doesn't respect the following condition : delayTypeId < 0");
        }
        
    }
    
}