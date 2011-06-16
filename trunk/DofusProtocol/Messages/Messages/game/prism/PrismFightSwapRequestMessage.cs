// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'PrismFightSwapRequestMessage.xml' the '15/06/2011 01:39:08'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class PrismFightSwapRequestMessage : Message
	{
		public const uint Id = 5901;
		public override uint MessageId
		{
			get
			{
				return 5901;
			}
		}
		
		public int targetId;
		
		public PrismFightSwapRequestMessage()
		{
		}
		
		public PrismFightSwapRequestMessage(int targetId)
		{
			this.targetId = targetId;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteInt(targetId);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			targetId = reader.ReadInt();
			if ( targetId < 0 )
			{
				throw new Exception("Forbidden value on targetId = " + targetId + ", it doesn't respect the following condition : targetId < 0");
			}
		}
	}
}
