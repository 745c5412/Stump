// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'GameActionFightShieldPointsVariationMessage.xml' the '15/06/2011 01:38:41'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class GameActionFightShieldPointsVariationMessage : AbstractGameActionMessage
	{
		public const uint Id = 6220;
		public override uint MessageId
		{
			get
			{
				return 6220;
			}
		}
		
		public int targetId;
		public short delta;
		
		public GameActionFightShieldPointsVariationMessage()
		{
		}
		
		public GameActionFightShieldPointsVariationMessage(short actionId, int sourceId, int targetId, short delta)
			 : base(actionId, sourceId)
		{
			this.targetId = targetId;
			this.delta = delta;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			base.Serialize(writer);
			writer.WriteInt(targetId);
			writer.WriteShort(delta);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			base.Deserialize(reader);
			targetId = reader.ReadInt();
			delta = reader.ReadShort();
		}
	}
}
