// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'GetPartInfoMessage.xml' the '15/06/2011 01:39:10'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class GetPartInfoMessage : Message
	{
		public const uint Id = 1506;
		public override uint MessageId
		{
			get
			{
				return 1506;
			}
		}
		
		public string id;
		
		public GetPartInfoMessage()
		{
		}
		
		public GetPartInfoMessage(string id)
		{
			this.id = id;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteUTF(id);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			id = reader.ReadUTF();
		}
	}
}
