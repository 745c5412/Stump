// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'SetEnablePVPRequestMessage.xml' the '15/06/2011 01:39:09'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class SetEnablePVPRequestMessage : Message
	{
		public const uint Id = 1810;
		public override uint MessageId
		{
			get
			{
				return 1810;
			}
		}
		
		public bool enable;
		
		public SetEnablePVPRequestMessage()
		{
		}
		
		public SetEnablePVPRequestMessage(bool enable)
		{
			this.enable = enable;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteBoolean(enable);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			enable = reader.ReadBoolean();
		}
	}
}
