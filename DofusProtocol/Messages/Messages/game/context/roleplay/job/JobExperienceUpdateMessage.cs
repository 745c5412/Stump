

// Generated on 10/26/2014 23:29:26
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class JobExperienceUpdateMessage : Message
    {
        public const uint Id = 5654;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.JobExperience experiencesUpdate;
        
        public JobExperienceUpdateMessage()
        {
        }
        
        public JobExperienceUpdateMessage(Types.JobExperience experiencesUpdate)
        {
            this.experiencesUpdate = experiencesUpdate;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            experiencesUpdate.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            experiencesUpdate = new Types.JobExperience();
            experiencesUpdate.Deserialize(reader);
        }
        
        public override int GetSerializationSize()
        {
            return experiencesUpdate.GetSerializationSize();
        }
        
    }
    
}