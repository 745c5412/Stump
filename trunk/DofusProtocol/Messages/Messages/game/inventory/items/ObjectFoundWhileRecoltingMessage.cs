// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'ObjectFoundWhileRecoltingMessage.xml' the '30/06/2011 11:40:20'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class ObjectFoundWhileRecoltingMessage : Message
	{
		public const uint Id = 6017;
		public override uint MessageId
		{
			get
			{
				return 6017;
			}
		}
		
		public int genericId;
		public int quantity;
		public int ressourceGenericId;
		
		public ObjectFoundWhileRecoltingMessage()
		{
		}
		
		public ObjectFoundWhileRecoltingMessage(int genericId, int quantity, int ressourceGenericId)
		{
			this.genericId = genericId;
			this.quantity = quantity;
			this.ressourceGenericId = ressourceGenericId;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteInt(genericId);
			writer.WriteInt(quantity);
			writer.WriteInt(ressourceGenericId);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			genericId = reader.ReadInt();
			if ( genericId < 0 )
			{
				throw new Exception("Forbidden value on genericId = " + genericId + ", it doesn't respect the following condition : genericId < 0");
			}
			quantity = reader.ReadInt();
			if ( quantity < 0 )
			{
				throw new Exception("Forbidden value on quantity = " + quantity + ", it doesn't respect the following condition : quantity < 0");
			}
			ressourceGenericId = reader.ReadInt();
			if ( ressourceGenericId < 0 )
			{
				throw new Exception("Forbidden value on ressourceGenericId = " + ressourceGenericId + ", it doesn't respect the following condition : ressourceGenericId < 0");
			}
		}
	}
}
