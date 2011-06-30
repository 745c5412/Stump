// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'AdminQuietCommandMessage.xml' the '30/06/2011 11:40:08'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class AdminQuietCommandMessage : AdminCommandMessage
	{
		public const uint Id = 5662;
		public override uint MessageId
		{
			get
			{
				return 5662;
			}
		}
		
		
		public AdminQuietCommandMessage()
		{
		}
		
		public AdminQuietCommandMessage(string content)
			 : base(content)
		{
		}
		
		public override void Serialize(IDataWriter writer)
		{
			base.Serialize(writer);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			base.Deserialize(reader);
		}
	}
}
