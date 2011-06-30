// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'ObjectsAddedMessage.xml' the '30/06/2011 11:40:20'
using System;
using Stump.Core.IO;
using System.Collections.Generic;
using System.Linq;

namespace Stump.DofusProtocol.Messages
{
	public class ObjectsAddedMessage : Message
	{
		public const uint Id = 6033;
		public override uint MessageId
		{
			get
			{
				return 6033;
			}
		}
		
		public IEnumerable<Types.ObjectItem> @object;
		
		public ObjectsAddedMessage()
		{
		}
		
		public ObjectsAddedMessage(IEnumerable<Types.ObjectItem> @object)
		{
			this.@object = @object;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteUShort((ushort)@object.Count());
			foreach (var entry in @object)
			{
				entry.Serialize(writer);
			}
		}
		
		public override void Deserialize(IDataReader reader)
		{
			int limit = reader.ReadUShort();
			@object = new Types.ObjectItem[limit];
			for (int i = 0; i < limit; i++)
			{
				(@object as Types.ObjectItem[])[i] = new Types.ObjectItem();
				(@object as Types.ObjectItem[])[i].Deserialize(reader);
			}
		}
	}
}
