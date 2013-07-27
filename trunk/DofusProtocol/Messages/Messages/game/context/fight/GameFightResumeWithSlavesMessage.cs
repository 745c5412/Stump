

// Generated on 07/26/2013 22:50:54
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameFightResumeWithSlavesMessage : GameFightResumeMessage
    {
        public const uint Id = 6215;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.GameFightResumeSlaveInfo> slavesInfo;
        
        public GameFightResumeWithSlavesMessage()
        {
        }
        
        public GameFightResumeWithSlavesMessage(IEnumerable<Types.FightDispellableEffectExtendedInformations> effects, IEnumerable<Types.GameActionMark> marks, short gameTurn, IEnumerable<Types.GameFightSpellCooldown> spellCooldowns, sbyte summonCount, sbyte bombCount, IEnumerable<Types.GameFightResumeSlaveInfo> slavesInfo)
         : base(effects, marks, gameTurn, spellCooldowns, summonCount, bombCount)
        {
            this.slavesInfo = slavesInfo;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUShort((ushort)slavesInfo.Count());
            foreach (var entry in slavesInfo)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            slavesInfo = new Types.GameFightResumeSlaveInfo[limit];
            for (int i = 0; i < limit; i++)
            {
                 (slavesInfo as Types.GameFightResumeSlaveInfo[])[i] = new Types.GameFightResumeSlaveInfo();
                 (slavesInfo as Types.GameFightResumeSlaveInfo[])[i].Deserialize(reader);
            }
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(short) + slavesInfo.Sum(x => x.GetSerializationSize());
        }
        
    }
    
}