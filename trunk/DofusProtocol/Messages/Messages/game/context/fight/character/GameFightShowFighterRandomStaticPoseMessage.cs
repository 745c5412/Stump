// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'GameFightShowFighterRandomStaticPoseMessage.xml' the '15/06/2011 01:38:49'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class GameFightShowFighterRandomStaticPoseMessage : GameFightShowFighterMessage
	{
		public const uint Id = 6218;
		public override uint MessageId
		{
			get
			{
				return 6218;
			}
		}
		
		
		public GameFightShowFighterRandomStaticPoseMessage()
		{
		}
		
		public GameFightShowFighterRandomStaticPoseMessage(Types.GameFightFighterInformations informations)
			 : base(informations)
		{
		}
		
		public override void Serialize(IDataWriter writer)
		{
			base.Serialize(writer);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			base.Deserialize(reader);
		}
	}
}
