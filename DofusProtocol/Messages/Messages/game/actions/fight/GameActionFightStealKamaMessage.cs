

// Generated on 02/17/2017 01:57:37
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameActionFightStealKamaMessage : AbstractGameActionMessage
    {
        public const uint Id = 5535;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public double targetId;
        public long amount;
        
        public GameActionFightStealKamaMessage()
        {
        }
        
        public GameActionFightStealKamaMessage(short actionId, double sourceId, double targetId, long amount)
         : base(actionId, sourceId)
        {
            this.targetId = targetId;
            this.amount = amount;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteDouble(targetId);
            writer.WriteVarLong(amount);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            targetId = reader.ReadDouble();
            if (targetId < -9007199254740990 || targetId > 9007199254740990)
                throw new Exception("Forbidden value on targetId = " + targetId + ", it doesn't respect the following condition : targetId < -9007199254740990 || targetId > 9007199254740990");
            amount = reader.ReadVarLong();
            if (amount < 0 || amount > 9007199254740990)
                throw new Exception("Forbidden value on amount = " + amount + ", it doesn't respect the following condition : amount < 0 || amount > 9007199254740990");
        }
        
    }
    
}