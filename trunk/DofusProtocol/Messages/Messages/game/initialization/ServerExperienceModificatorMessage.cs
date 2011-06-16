// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'ServerExperienceModificatorMessage.xml' the '15/06/2011 01:39:01'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class ServerExperienceModificatorMessage : Message
	{
		public const uint Id = 6237;
		public override uint MessageId
		{
			get
			{
				return 6237;
			}
		}
		
		public short experiencePercent;
		
		public ServerExperienceModificatorMessage()
		{
		}
		
		public ServerExperienceModificatorMessage(short experiencePercent)
		{
			this.experiencePercent = experiencePercent;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteShort(experiencePercent);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			experiencePercent = reader.ReadShort();
			if ( experiencePercent < 0 )
			{
				throw new Exception("Forbidden value on experiencePercent = " + experiencePercent + ", it doesn't respect the following condition : experiencePercent < 0");
			}
		}
	}
}
