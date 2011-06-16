// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'ExchangeObjectTransfertListFromInvMessage.xml' the '15/06/2011 01:39:03'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class ExchangeObjectTransfertListFromInvMessage : Message
	{
		public const uint Id = 6183;
		public override uint MessageId
		{
			get
			{
				return 6183;
			}
		}
		
		public int[] ids;
		
		public ExchangeObjectTransfertListFromInvMessage()
		{
		}
		
		public ExchangeObjectTransfertListFromInvMessage(int[] ids)
		{
			this.ids = ids;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteUShort((ushort)ids.Length);
			for (int i = 0; i < ids.Length; i++)
			{
				writer.WriteInt(ids[i]);
			}
		}
		
		public override void Deserialize(IDataReader reader)
		{
			int limit = reader.ReadUShort();
			ids = new int[limit];
			for (int i = 0; i < limit; i++)
			{
				ids[i] = reader.ReadInt();
			}
		}
	}
}
