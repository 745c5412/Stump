// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'GameRolePlayShowActorMessage.xml' the '15/06/2011 01:38:50'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class GameRolePlayShowActorMessage : Message
	{
		public const uint Id = 5632;
		public override uint MessageId
		{
			get
			{
				return 5632;
			}
		}
		
		public Types.GameRolePlayActorInformations informations;
		
		public GameRolePlayShowActorMessage()
		{
		}
		
		public GameRolePlayShowActorMessage(Types.GameRolePlayActorInformations informations)
		{
			this.informations = informations;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteShort(informations.TypeId);
			informations.Serialize(writer);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			informations = Types.ProtocolTypeManager.GetInstance<Types.GameRolePlayActorInformations>(reader.ReadShort());
			informations.Deserialize(reader);
		}
	}
}
