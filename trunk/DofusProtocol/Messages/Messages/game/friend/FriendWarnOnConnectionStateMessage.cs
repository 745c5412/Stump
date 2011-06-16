// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'FriendWarnOnConnectionStateMessage.xml' the '15/06/2011 01:38:58'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class FriendWarnOnConnectionStateMessage : Message
	{
		public const uint Id = 5630;
		public override uint MessageId
		{
			get
			{
				return 5630;
			}
		}
		
		public bool enable;
		
		public FriendWarnOnConnectionStateMessage()
		{
		}
		
		public FriendWarnOnConnectionStateMessage(bool enable)
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
