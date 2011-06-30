// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'ObjectDropMessage.xml' the '30/06/2011 11:40:20'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class ObjectDropMessage : Message
	{
		public const uint Id = 3005;
		public override uint MessageId
		{
			get
			{
				return 3005;
			}
		}
		
		public int objectUID;
		public int quantity;
		
		public ObjectDropMessage()
		{
		}
		
		public ObjectDropMessage(int objectUID, int quantity)
		{
			this.objectUID = objectUID;
			this.quantity = quantity;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteInt(objectUID);
			writer.WriteInt(quantity);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			objectUID = reader.ReadInt();
			if ( objectUID < 0 )
			{
				throw new Exception("Forbidden value on objectUID = " + objectUID + ", it doesn't respect the following condition : objectUID < 0");
			}
			quantity = reader.ReadInt();
			if ( quantity < 0 )
			{
				throw new Exception("Forbidden value on quantity = " + quantity + ", it doesn't respect the following condition : quantity < 0");
			}
		}
	}
}
