

// Generated on 12/26/2016 21:58:06
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ShortcutBarReplacedMessage : Message
    {
        public const uint Id = 6706;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte barType;
        public Types.Shortcut shortcut;
        
        public ShortcutBarReplacedMessage()
        {
        }
        
        public ShortcutBarReplacedMessage(sbyte barType, Types.Shortcut shortcut)
        {
            this.barType = barType;
            this.shortcut = shortcut;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(barType);
            writer.WriteShort(shortcut.TypeId);
            shortcut.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            barType = reader.ReadSByte();
            if (barType < 0)
                throw new Exception("Forbidden value on barType = " + barType + ", it doesn't respect the following condition : barType < 0");
            shortcut = Types.ProtocolTypeManager.GetInstance<Types.Shortcut>(reader.ReadShort());
            shortcut.Deserialize(reader);
        }
        
    }
    
}