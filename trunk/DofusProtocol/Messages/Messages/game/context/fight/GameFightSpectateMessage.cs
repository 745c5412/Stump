// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'GameFightSpectateMessage.xml' the '15/06/2011 01:38:48'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class GameFightSpectateMessage : Message
	{
		public const uint Id = 6069;
		public override uint MessageId
		{
			get
			{
				return 6069;
			}
		}
		
		public Types.FightDispellableEffectExtendedInformations[] effects;
		public Types.GameActionMark[] marks;
		public short gameTurn;
		
		public GameFightSpectateMessage()
		{
		}
		
		public GameFightSpectateMessage(Types.FightDispellableEffectExtendedInformations[] effects, Types.GameActionMark[] marks, short gameTurn)
		{
			this.effects = effects;
			this.marks = marks;
			this.gameTurn = gameTurn;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteUShort((ushort)effects.Length);
			for (int i = 0; i < effects.Length; i++)
			{
				writer.WriteShort(effects[i].TypeId);
				effects[i].Serialize(writer);
			}
			writer.WriteUShort((ushort)marks.Length);
			for (int i = 0; i < marks.Length; i++)
			{
				marks[i].Serialize(writer);
			}
			writer.WriteShort(gameTurn);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			int limit = reader.ReadUShort();
			effects = new Types.FightDispellableEffectExtendedInformations[limit];
			for (int i = 0; i < limit; i++)
			{
				effects[i] = Types.ProtocolTypeManager.GetInstance<Types.FightDispellableEffectExtendedInformations>(reader.ReadShort());
				effects[i].Deserialize(reader);
			}
			limit = reader.ReadUShort();
			marks = new Types.GameActionMark[limit];
			for (int i = 0; i < limit; i++)
			{
				marks[i] = new Types.GameActionMark();
				marks[i].Deserialize(reader);
			}
			gameTurn = reader.ReadShort();
			if ( gameTurn < 0 )
			{
				throw new Exception("Forbidden value on gameTurn = " + gameTurn + ", it doesn't respect the following condition : gameTurn < 0");
			}
		}
	}
}
