// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'PrismInfoJoinLeaveRequestMessage.xml' the '30/06/2011 11:40:21'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class PrismInfoJoinLeaveRequestMessage : Message
	{
		public const uint Id = 5844;
		public override uint MessageId
		{
			get
			{
				return 5844;
			}
		}
		
		public bool join;
		
		public PrismInfoJoinLeaveRequestMessage()
		{
		}
		
		public PrismInfoJoinLeaveRequestMessage(bool join)
		{
			this.join = join;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteBoolean(join);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			join = reader.ReadBoolean();
		}
	}
}
