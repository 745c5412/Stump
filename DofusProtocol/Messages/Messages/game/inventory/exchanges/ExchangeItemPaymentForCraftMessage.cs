

// Generated on 03/02/2014 20:42:50
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeItemPaymentForCraftMessage : Message
    {
        public const uint Id = 5831;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool onlySuccess;
        public Types.ObjectItemNotInContainer @object;
        
        public ExchangeItemPaymentForCraftMessage()
        {
        }
        
        public ExchangeItemPaymentForCraftMessage(bool onlySuccess, Types.ObjectItemNotInContainer @object)
        {
            this.onlySuccess = onlySuccess;
            this.@object = @object;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(onlySuccess);
            @object.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            onlySuccess = reader.ReadBoolean();
            @object = new Types.ObjectItemNotInContainer();
            @object.Deserialize(reader);
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(bool) + @object.GetSerializationSize();
        }
        
    }
    
}