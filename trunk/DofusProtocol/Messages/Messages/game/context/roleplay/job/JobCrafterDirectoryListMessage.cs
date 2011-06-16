// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'JobCrafterDirectoryListMessage.xml' the '15/06/2011 01:38:52'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class JobCrafterDirectoryListMessage : Message
	{
		public const uint Id = 6046;
		public override uint MessageId
		{
			get
			{
				return 6046;
			}
		}
		
		public Types.JobCrafterDirectoryListEntry[] listEntries;
		
		public JobCrafterDirectoryListMessage()
		{
		}
		
		public JobCrafterDirectoryListMessage(Types.JobCrafterDirectoryListEntry[] listEntries)
		{
			this.listEntries = listEntries;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteUShort((ushort)listEntries.Length);
			for (int i = 0; i < listEntries.Length; i++)
			{
				listEntries[i].Serialize(writer);
			}
		}
		
		public override void Deserialize(IDataReader reader)
		{
			int limit = reader.ReadUShort();
			listEntries = new Types.JobCrafterDirectoryListEntry[limit];
			for (int i = 0; i < limit; i++)
			{
				listEntries[i] = new Types.JobCrafterDirectoryListEntry();
				listEntries[i].Deserialize(reader);
			}
		}
	}
}
