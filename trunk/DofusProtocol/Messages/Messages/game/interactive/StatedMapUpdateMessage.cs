// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'StatedMapUpdateMessage.xml' the '15/06/2011 01:39:01'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class StatedMapUpdateMessage : Message
	{
		public const uint Id = 5716;
		public override uint MessageId
		{
			get
			{
				return 5716;
			}
		}
		
		public Types.StatedElement[] statedElements;
		
		public StatedMapUpdateMessage()
		{
		}
		
		public StatedMapUpdateMessage(Types.StatedElement[] statedElements)
		{
			this.statedElements = statedElements;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteUShort((ushort)statedElements.Length);
			for (int i = 0; i < statedElements.Length; i++)
			{
				statedElements[i].Serialize(writer);
			}
		}
		
		public override void Deserialize(IDataReader reader)
		{
			int limit = reader.ReadUShort();
			statedElements = new Types.StatedElement[limit];
			for (int i = 0; i < limit; i++)
			{
				statedElements[i] = new Types.StatedElement();
				statedElements[i].Deserialize(reader);
			}
		}
	}
}
