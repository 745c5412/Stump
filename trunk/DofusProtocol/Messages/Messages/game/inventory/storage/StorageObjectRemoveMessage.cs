// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'StorageObjectRemoveMessage.xml' the '30/06/2011 11:40:20'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class StorageObjectRemoveMessage : Message
	{
		public const uint Id = 5648;
		public override uint MessageId
		{
			get
			{
				return 5648;
			}
		}
		
		public int objectUID;
		
		public StorageObjectRemoveMessage()
		{
		}
		
		public StorageObjectRemoveMessage(int objectUID)
		{
			this.objectUID = objectUID;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteInt(objectUID);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			objectUID = reader.ReadInt();
			if ( objectUID < 0 )
			{
				throw new Exception("Forbidden value on objectUID = " + objectUID + ", it doesn't respect the following condition : objectUID < 0");
			}
		}
	}
}
