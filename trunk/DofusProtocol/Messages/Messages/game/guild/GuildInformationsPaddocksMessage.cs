// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'GuildInformationsPaddocksMessage.xml' the '15/06/2011 01:38:59'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class GuildInformationsPaddocksMessage : Message
	{
		public const uint Id = 5959;
		public override uint MessageId
		{
			get
			{
				return 5959;
			}
		}
		
		public byte nbPaddockMax;
		public Types.PaddockContentInformations[] paddocksInformations;
		
		public GuildInformationsPaddocksMessage()
		{
		}
		
		public GuildInformationsPaddocksMessage(byte nbPaddockMax, Types.PaddockContentInformations[] paddocksInformations)
		{
			this.nbPaddockMax = nbPaddockMax;
			this.paddocksInformations = paddocksInformations;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteByte(nbPaddockMax);
			writer.WriteUShort((ushort)paddocksInformations.Length);
			for (int i = 0; i < paddocksInformations.Length; i++)
			{
				paddocksInformations[i].Serialize(writer);
			}
		}
		
		public override void Deserialize(IDataReader reader)
		{
			nbPaddockMax = reader.ReadByte();
			if ( nbPaddockMax < 0 )
			{
				throw new Exception("Forbidden value on nbPaddockMax = " + nbPaddockMax + ", it doesn't respect the following condition : nbPaddockMax < 0");
			}
			int limit = reader.ReadUShort();
			paddocksInformations = new Types.PaddockContentInformations[limit];
			for (int i = 0; i < limit; i++)
			{
				paddocksInformations[i] = new Types.PaddockContentInformations();
				paddocksInformations[i].Deserialize(reader);
			}
		}
	}
}
