// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'EmotePlayMessage.xml' the '30/06/2011 11:40:13'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class EmotePlayMessage : EmotePlayAbstractMessage
	{
		public const uint Id = 5683;
		public override uint MessageId
		{
			get
			{
				return 5683;
			}
		}
		
		public int actorId;
		public int accountId;
		
		public EmotePlayMessage()
		{
		}
		
		public EmotePlayMessage(byte emoteId, byte duration, int actorId, int accountId)
			 : base(emoteId, duration)
		{
			this.actorId = actorId;
			this.accountId = accountId;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			base.Serialize(writer);
			writer.WriteInt(actorId);
			writer.WriteInt(accountId);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			base.Deserialize(reader);
			actorId = reader.ReadInt();
			accountId = reader.ReadInt();
		}
	}
}
