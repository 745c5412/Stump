// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'ObjectJobAddedMessage.xml' the '30/06/2011 11:40:20'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class ObjectJobAddedMessage : Message
	{
		public const uint Id = 6014;
		public override uint MessageId
		{
			get
			{
				return 6014;
			}
		}
		
		public byte jobId;
		
		public ObjectJobAddedMessage()
		{
		}
		
		public ObjectJobAddedMessage(byte jobId)
		{
			this.jobId = jobId;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteByte(jobId);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			jobId = reader.ReadByte();
			if ( jobId < 0 )
			{
				throw new Exception("Forbidden value on jobId = " + jobId + ", it doesn't respect the following condition : jobId < 0");
			}
		}
	}
}
