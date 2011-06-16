// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'ContactLookErrorMessage.xml' the '15/06/2011 01:39:09'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class ContactLookErrorMessage : Message
	{
		public const uint Id = 6045;
		public override uint MessageId
		{
			get
			{
				return 6045;
			}
		}
		
		public int requestId;
		
		public ContactLookErrorMessage()
		{
		}
		
		public ContactLookErrorMessage(int requestId)
		{
			this.requestId = requestId;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteInt(requestId);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			requestId = reader.ReadInt();
			if ( requestId < 0 )
			{
				throw new Exception("Forbidden value on requestId = " + requestId + ", it doesn't respect the following condition : requestId < 0");
			}
		}
	}
}
