

// Generated on 09/01/2014 15:52:00
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class JobCrafterDirectoryDefineSettingsMessage : Message
    {
        public const uint Id = 5649;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.JobCrafterDirectorySettings settings;
        
        public JobCrafterDirectoryDefineSettingsMessage()
        {
        }
        
        public JobCrafterDirectoryDefineSettingsMessage(Types.JobCrafterDirectorySettings settings)
        {
            this.settings = settings;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            settings.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            settings = new Types.JobCrafterDirectorySettings();
            settings.Deserialize(reader);
        }
        
        public override int GetSerializationSize()
        {
            return settings.GetSerializationSize();
        }
        
    }
    
}