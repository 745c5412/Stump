// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'InventoryWeightMessage.xml' the '30/06/2011 11:40:20'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class InventoryWeightMessage : Message
	{
		public const uint Id = 3009;
		public override uint MessageId
		{
			get
			{
				return 3009;
			}
		}
		
		public int weight;
		public int weightMax;
		
		public InventoryWeightMessage()
		{
		}
		
		public InventoryWeightMessage(int weight, int weightMax)
		{
			this.weight = weight;
			this.weightMax = weightMax;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteInt(weight);
			writer.WriteInt(weightMax);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			weight = reader.ReadInt();
			if ( weight < 0 )
			{
				throw new Exception("Forbidden value on weight = " + weight + ", it doesn't respect the following condition : weight < 0");
			}
			weightMax = reader.ReadInt();
			if ( weightMax < 0 )
			{
				throw new Exception("Forbidden value on weightMax = " + weightMax + ", it doesn't respect the following condition : weightMax < 0");
			}
		}
	}
}
