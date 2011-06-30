// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'GameActionFightReflectSpellMessage.xml' the '30/06/2011 11:40:09'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class GameActionFightReflectSpellMessage : AbstractGameActionMessage
	{
		public const uint Id = 5531;
		public override uint MessageId
		{
			get
			{
				return 5531;
			}
		}
		
		public int targetId;
		
		public GameActionFightReflectSpellMessage()
		{
		}
		
		public GameActionFightReflectSpellMessage(short actionId, int sourceId, int targetId)
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
