

// Generated on 01/04/2015 11:54:48
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class ServerSessionConstantString : ServerSessionConstant
    {
        public const short Id = 436;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public string value;
        
        public ServerSessionConstantString()
        {
        }
        
        public ServerSessionConstantString(short id, string value)
         : base(id)
        {
            this.value = value;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUTF(value);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            value = reader.ReadUTF();
        }
        
        
    }
    
}