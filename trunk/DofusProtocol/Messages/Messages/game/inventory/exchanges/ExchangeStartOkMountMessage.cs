// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'ExchangeStartOkMountMessage.xml' the '15/06/2011 01:39:05'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class ExchangeStartOkMountMessage : ExchangeStartOkMountWithOutPaddockMessage
	{
		public const uint Id = 5979;
		public override uint MessageId
		{
			get
			{
				return 5979;
			}
		}
		
		public Types.MountClientData[] paddockedMountsDescription;
		
		public ExchangeStartOkMountMessage()
		{
		}
		
		public ExchangeStartOkMountMessage(Types.MountClientData[] stabledMountsDescription, Types.MountClientData[] paddockedMountsDescription)
			 : base(stabledMountsDescription)
		{
			this.paddockedMountsDescription = paddockedMountsDescription;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			base.Serialize(writer);
			writer.WriteUShort((ushort)paddockedMountsDescription.Length);
			for (int i = 0; i < paddockedMountsDescription.Length; i++)
			{
				paddockedMountsDescription[i].Serialize(writer);
			}
		}
		
		public override void Deserialize(IDataReader reader)
		{
			base.Deserialize(reader);
			int limit = reader.ReadUShort();
			paddockedMountsDescription = new Types.MountClientData[limit];
			for (int i = 0; i < limit; i++)
			{
				paddockedMountsDescription[i] = new Types.MountClientData();
				paddockedMountsDescription[i].Deserialize(reader);
			}
		}
	}
}
