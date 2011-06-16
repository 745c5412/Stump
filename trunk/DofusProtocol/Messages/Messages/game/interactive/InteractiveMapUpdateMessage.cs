// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'InteractiveMapUpdateMessage.xml' the '15/06/2011 01:39:01'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class InteractiveMapUpdateMessage : Message
	{
		public const uint Id = 5002;
		public override uint MessageId
		{
			get
			{
				return 5002;
			}
		}
		
		public Types.InteractiveElement[] interactiveElements;
		
		public InteractiveMapUpdateMessage()
		{
		}
		
		public InteractiveMapUpdateMessage(Types.InteractiveElement[] interactiveElements)
		{
			this.interactiveElements = interactiveElements;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteUShort((ushort)interactiveElements.Length);
			for (int i = 0; i < interactiveElements.Length; i++)
			{
				interactiveElements[i].Serialize(writer);
			}
		}
		
		public override void Deserialize(IDataReader reader)
		{
			int limit = reader.ReadUShort();
			interactiveElements = new Types.InteractiveElement[limit];
			for (int i = 0; i < limit; i++)
			{
				interactiveElements[i] = new Types.InteractiveElement();
				interactiveElements[i].Deserialize(reader);
			}
		}
	}
}
