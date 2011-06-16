// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'IgnoredAddFailureMessage.xml' the '15/06/2011 01:38:58'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class IgnoredAddFailureMessage : Message
	{
		public const uint Id = 5679;
		public override uint MessageId
		{
			get
			{
				return 5679;
			}
		}
		
		public byte reason;
		
		public IgnoredAddFailureMessage()
		{
		}
		
		public IgnoredAddFailureMessage(byte reason)
		{
			this.reason = reason;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteByte(reason);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			reason = reader.ReadByte();
			if ( reason < 0 )
			{
				throw new Exception("Forbidden value on reason = " + reason + ", it doesn't respect the following condition : reason < 0");
			}
		}
	}
}
