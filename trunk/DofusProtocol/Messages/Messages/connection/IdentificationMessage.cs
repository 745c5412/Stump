// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'IdentificationMessage.xml' the '15/06/2011 01:38:39'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class IdentificationMessage : Message
	{
		public const uint Id = 4;
		public override uint MessageId
		{
			get
			{
				return 4;
			}
		}
		
		public Types.Version version;
		public string login;
		public string password;
		public bool autoconnect;
		
		public IdentificationMessage()
		{
		}
		
		public IdentificationMessage(Types.Version version, string login, string password, bool autoconnect)
		{
			this.version = version;
			this.login = login;
			this.password = password;
			this.autoconnect = autoconnect;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			version.Serialize(writer);
			writer.WriteUTF(login);
			writer.WriteUTF(password);
			writer.WriteBoolean(autoconnect);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			version = new Types.Version();
			version.Deserialize(reader);
			login = reader.ReadUTF();
			password = reader.ReadUTF();
			autoconnect = reader.ReadBoolean();
		}
	}
}
