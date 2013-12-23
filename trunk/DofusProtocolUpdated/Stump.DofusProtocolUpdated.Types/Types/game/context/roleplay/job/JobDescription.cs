

// Generated on 12/12/2013 16:57:32
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class JobDescription
    {
        public const short Id = 101;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public sbyte jobId;
        public IEnumerable<Types.SkillActionDescription> skills;
        
        public JobDescription()
        {
        }
        
        public JobDescription(sbyte jobId, IEnumerable<Types.SkillActionDescription> skills)
        {
            this.jobId = jobId;
            this.skills = skills;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(jobId);
            writer.WriteUShort((ushort)skills.Count());
            foreach (var entry in skills)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            jobId = reader.ReadSByte();
            if (jobId < 0)
                throw new Exception("Forbidden value on jobId = " + jobId + ", it doesn't respect the following condition : jobId < 0");
            var limit = reader.ReadUShort();
            skills = new Types.SkillActionDescription[limit];
            for (int i = 0; i < limit; i++)
            {
                 (skills as Types.SkillActionDescription[])[i] = Types.ProtocolTypeManager.GetInstance<Types.SkillActionDescription>(reader.ReadShort());
                 (skills as Types.SkillActionDescription[])[i].Deserialize(reader);
            }
        }
        
        public virtual int GetSerializationSize()
        {
            return sizeof(sbyte) + sizeof(short) + skills.Sum(x => sizeof(short) + x.GetSerializationSize());
        }
        
    }
    
}