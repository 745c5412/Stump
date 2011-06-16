// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'GameEntitiesDispositionMessage.xml' the '15/06/2011 01:38:46'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class GameEntitiesDispositionMessage : Message
	{
		public const uint Id = 5696;
		public override uint MessageId
		{
			get
			{
				return 5696;
			}
		}
		
		public Types.IdentifiedEntityDispositionInformations[] dispositions;
		
		public GameEntitiesDispositionMessage()
		{
		}
		
		public GameEntitiesDispositionMessage(Types.IdentifiedEntityDispositionInformations[] dispositions)
		{
			this.dispositions = dispositions;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteUShort((ushort)dispositions.Length);
			for (int i = 0; i < dispositions.Length; i++)
			{
				writer.WriteShort(dispositions[i].TypeId);
				dispositions[i].Serialize(writer);
			}
		}
		
		public override void Deserialize(IDataReader reader)
		{
			int limit = reader.ReadUShort();
			dispositions = new Types.IdentifiedEntityDispositionInformations[limit];
			for (int i = 0; i < limit; i++)
			{
				dispositions[i] = Types.ProtocolTypeManager.GetInstance<Types.IdentifiedEntityDispositionInformations>(reader.ReadShort());
				dispositions[i].Deserialize(reader);
			}
		}
	}
}
