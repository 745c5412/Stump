// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'ShortcutBarSwapErrorMessage.xml' the '15/06/2011 01:39:09'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class ShortcutBarSwapErrorMessage : Message
	{
		public const uint Id = 6226;
		public override uint MessageId
		{
			get
			{
				return 6226;
			}
		}
		
		public byte error;
		
		public ShortcutBarSwapErrorMessage()
		{
		}
		
		public ShortcutBarSwapErrorMessage(byte error)
		{
			this.error = error;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteByte(error);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			error = reader.ReadByte();
			if ( error < 0 )
			{
				throw new Exception("Forbidden value on error = " + error + ", it doesn't respect the following condition : error < 0");
			}
		}
	}
}
