// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'GameActionFightDeathMessage.xml' the '15/06/2011 01:38:40'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class GameActionFightDeathMessage : AbstractGameActionMessage
	{
		public const uint Id = 1099;
		public override uint MessageId
		{
			get
			{
				return 1099;
			}
		}
		
		public int targetId;
		
		public GameActionFightDeathMessage()
		{
		}
		
		public GameActionFightDeathMessage(short actionId, int sourceId, int targetId)
			 : base(actionId, sourceId)
		{
			this.targetId = targetId;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			base.Serialize(writer);
			writer.WriteInt(targetId);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			base.Deserialize(reader);
			targetId = reader.ReadInt();
		}
	}
}
