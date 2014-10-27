

// Generated on 10/27/2014 19:57:53
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GuildFightPlayersHelpersLeaveMessage : Message
    {
        public const uint Id = 5719;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public double fightId;
        public int playerId;
        
        public GuildFightPlayersHelpersLeaveMessage()
        {
        }
        
        public GuildFightPlayersHelpersLeaveMessage(double fightId, int playerId)
        {
            this.fightId = fightId;
            this.playerId = playerId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteDouble(fightId);
            writer.WriteInt(playerId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            fightId = reader.ReadDouble();
            if (fightId < 0 || fightId > 9.007199254740992E15)
                throw new Exception("Forbidden value on fightId = " + fightId + ", it doesn't respect the following condition : fightId < 0 || fightId > 9.007199254740992E15");
            playerId = reader.ReadInt();
            if (playerId < 0)
                throw new Exception("Forbidden value on playerId = " + playerId + ", it doesn't respect the following condition : playerId < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(double) + sizeof(int);
        }
        
    }
    
}