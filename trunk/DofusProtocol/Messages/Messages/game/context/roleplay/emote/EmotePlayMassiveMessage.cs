// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'EmotePlayMassiveMessage.xml' the '30/06/2011 11:40:13'
using System;
using Stump.Core.IO;
using System.Collections.Generic;
using System.Linq;

namespace Stump.DofusProtocol.Messages
{
	public class EmotePlayMassiveMessage : EmotePlayAbstractMessage
	{
		public const uint Id = 5691;
		public override uint MessageId
		{
			get
			{
				return 5691;
			}
		}
		
		public IEnumerable<int> actorIds;
		
		public EmotePlayMassiveMessage()
		{
		}
		
		public EmotePlayMassiveMessage(byte emoteId, byte duration, IEnumerable<int> actorIds)
			 : base(emoteId, duration)
		{
			this.actorIds = actorIds;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			base.Serialize(writer);
			writer.WriteUShort((ushort)actorIds.Count());
			foreach (var entry in actorIds)
			{
				writer.WriteInt(entry);
			}
		}
		
		public override void Deserialize(IDataReader reader)
		{
			base.Deserialize(reader);
			int limit = reader.ReadUShort();
			actorIds = new int[limit];
			for (int i = 0; i < limit; i++)
			{
				(actorIds as int[])[i] = reader.ReadInt();
			}
		}
	}
}
