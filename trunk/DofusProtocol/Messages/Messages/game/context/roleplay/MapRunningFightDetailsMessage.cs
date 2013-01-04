
// Generated on 01/04/2013 14:35:47
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class MapRunningFightDetailsMessage : Message
    {
        public const uint Id = 5751;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int fightId;
        public IEnumerable<string> names;
        public IEnumerable<short> levels;
        public sbyte teamSwap;
        public IEnumerable<bool> alives;
        
        public MapRunningFightDetailsMessage()
        {
        }
        
        public MapRunningFightDetailsMessage(int fightId, IEnumerable<string> names, IEnumerable<short> levels, sbyte teamSwap, IEnumerable<bool> alives)
        {
            this.fightId = fightId;
            this.names = names;
            this.levels = levels;
            this.teamSwap = teamSwap;
            this.alives = alives;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(fightId);
            writer.WriteUShort((ushort)names.Count());
            foreach (var entry in names)
            {
                 writer.WriteUTF(entry);
            }
            writer.WriteUShort((ushort)levels.Count());
            foreach (var entry in levels)
            {
                 writer.WriteShort(entry);
            }
            writer.WriteSByte(teamSwap);
            writer.WriteUShort((ushort)alives.Count());
            foreach (var entry in alives)
            {
                 writer.WriteBoolean(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            fightId = reader.ReadInt();
            if (fightId < 0)
                throw new Exception("Forbidden value on fightId = " + fightId + ", it doesn't respect the following condition : fightId < 0");
            var limit = reader.ReadUShort();
            names = new string[limit];
            for (int i = 0; i < limit; i++)
            {
                 (names as string[])[i] = reader.ReadUTF();
            }
            limit = reader.ReadUShort();
            levels = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 (levels as short[])[i] = reader.ReadShort();
            }
            teamSwap = reader.ReadSByte();
            if (teamSwap < 0)
                throw new Exception("Forbidden value on teamSwap = " + teamSwap + ", it doesn't respect the following condition : teamSwap < 0");
            limit = reader.ReadUShort();
            alives = new bool[limit];
            for (int i = 0; i < limit; i++)
            {
                 (alives as bool[])[i] = reader.ReadBoolean();
            }
        }
        
    }
    
}