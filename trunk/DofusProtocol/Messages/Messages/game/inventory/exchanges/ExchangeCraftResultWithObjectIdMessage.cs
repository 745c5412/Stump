// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'ExchangeCraftResultWithObjectIdMessage.xml' the '15/06/2011 01:39:02'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class ExchangeCraftResultWithObjectIdMessage : ExchangeCraftResultMessage
	{
		public const uint Id = 6000;
		public override uint MessageId
		{
			get
			{
				return 6000;
			}
		}
		
		public int objectGenericId;
		
		public ExchangeCraftResultWithObjectIdMessage()
		{
		}
		
		public ExchangeCraftResultWithObjectIdMessage(byte craftResult, int objectGenericId)
			 : base(craftResult)
		{
			this.objectGenericId = objectGenericId;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			base.Serialize(writer);
			writer.WriteInt(objectGenericId);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			base.Deserialize(reader);
			objectGenericId = reader.ReadInt();
			if ( objectGenericId < 0 )
			{
				throw new Exception("Forbidden value on objectGenericId = " + objectGenericId + ", it doesn't respect the following condition : objectGenericId < 0");
			}
		}
	}
}
