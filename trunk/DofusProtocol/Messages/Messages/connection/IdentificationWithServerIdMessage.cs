// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'IdentificationWithServerIdMessage.xml' the '30/06/2011 11:40:08'
using System;
using Stump.Core.IO;
using System.Collections.Generic;

namespace Stump.DofusProtocol.Messages
{
	public class IdentificationWithServerIdMessage : IdentificationMessage
	{
		public const uint Id = 6194;
		public override uint MessageId
		{
			get
			{
				return 6194;
			}
		}
		
		public short serverId;
		
		public IdentificationWithServerIdMessage()
		{
		}
		
		public IdentificationWithServerIdMessage(Types.Version version, string login, string password, IEnumerable<Types.TrustCertificate> certificate, bool autoconnect, short serverId)
			 : base(version, login, password, certificate, autoconnect)
		{
			this.serverId = serverId;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			base.Serialize(writer);
			writer.WriteShort(serverId);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			base.Deserialize(reader);
			serverId = reader.ReadShort();
		}
	}
}
