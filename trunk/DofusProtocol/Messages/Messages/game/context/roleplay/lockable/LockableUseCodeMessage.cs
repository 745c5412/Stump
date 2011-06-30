// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'LockableUseCodeMessage.xml' the '30/06/2011 11:40:14'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class LockableUseCodeMessage : Message
	{
		public const uint Id = 5667;
		public override uint MessageId
		{
			get
			{
				return 5667;
			}
		}
		
		public string code;
		
		public LockableUseCodeMessage()
		{
		}
		
		public LockableUseCodeMessage(string code)
		{
			this.code = code;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteUTF(code);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			code = reader.ReadUTF();
		}
	}
}
