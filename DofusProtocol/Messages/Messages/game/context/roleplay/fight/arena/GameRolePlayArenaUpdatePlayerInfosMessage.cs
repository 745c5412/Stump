

// Generated on 11/16/2015 14:26:07
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameRolePlayArenaUpdatePlayerInfosMessage : Message
    {
        public const uint Id = 6301;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short rank;
        public short bestDailyRank;
        public short bestRank;
        public short victoryCount;
        public short arenaFightcount;
        
        public GameRolePlayArenaUpdatePlayerInfosMessage()
        {
        }
        
        public GameRolePlayArenaUpdatePlayerInfosMessage(short rank, short bestDailyRank, short bestRank, short victoryCount, short arenaFightcount)
        {
            this.rank = rank;
            this.bestDailyRank = bestDailyRank;
            this.bestRank = bestRank;
            this.victoryCount = victoryCount;
            this.arenaFightcount = arenaFightcount;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort(rank);
            writer.WriteVarShort(bestDailyRank);
            writer.WriteVarShort(bestRank);
            writer.WriteVarShort(victoryCount);
            writer.WriteVarShort(arenaFightcount);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            rank = reader.ReadVarShort();
            if (rank < 0 || rank > 2300)
                throw new Exception("Forbidden value on rank = " + rank + ", it doesn't respect the following condition : rank < 0 || rank > 2300");
            bestDailyRank = reader.ReadVarShort();
            if (bestDailyRank < 0 || bestDailyRank > 2300)
                throw new Exception("Forbidden value on bestDailyRank = " + bestDailyRank + ", it doesn't respect the following condition : bestDailyRank < 0 || bestDailyRank > 2300");
            bestRank = reader.ReadVarShort();
            if (bestRank < 0 || bestRank > 2300)
                throw new Exception("Forbidden value on bestRank = " + bestRank + ", it doesn't respect the following condition : bestRank < 0 || bestRank > 2300");
            victoryCount = reader.ReadVarShort();
            if (victoryCount < 0)
                throw new Exception("Forbidden value on victoryCount = " + victoryCount + ", it doesn't respect the following condition : victoryCount < 0");
            arenaFightcount = reader.ReadVarShort();
            if (arenaFightcount < 0)
                throw new Exception("Forbidden value on arenaFightcount = " + arenaFightcount + ", it doesn't respect the following condition : arenaFightcount < 0");
        }
        
    }
    
}