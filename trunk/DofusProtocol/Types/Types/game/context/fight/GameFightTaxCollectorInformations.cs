// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'GameFightTaxCollectorInformations.xml' the '30/06/2011 11:40:23'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
	public class GameFightTaxCollectorInformations : GameFightAIInformations
	{
		public const uint Id = 48;
		public override short TypeId
		{
			get
			{
				return 48;
			}
		}
		
		public short firstNameId;
		public short lastNameId;
		public short level;
		
		public GameFightTaxCollectorInformations()
		{
		}
		
		public GameFightTaxCollectorInformations(int contextualId, Types.EntityLook look, Types.EntityDispositionInformations disposition, byte teamId, bool alive, Types.GameFightMinimalStats stats, short firstNameId, short lastNameId, short level)
			 : base(contextualId, look, disposition, teamId, alive, stats)
		{
			this.firstNameId = firstNameId;
			this.lastNameId = lastNameId;
			this.level = level;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			base.Serialize(writer);
			writer.WriteShort(firstNameId);
			writer.WriteShort(lastNameId);
			writer.WriteShort(level);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			base.Deserialize(reader);
			firstNameId = reader.ReadShort();
			if ( firstNameId < 0 )
			{
				throw new Exception("Forbidden value on firstNameId = " + firstNameId + ", it doesn't respect the following condition : firstNameId < 0");
			}
			lastNameId = reader.ReadShort();
			if ( lastNameId < 0 )
			{
				throw new Exception("Forbidden value on lastNameId = " + lastNameId + ", it doesn't respect the following condition : lastNameId < 0");
			}
			level = reader.ReadShort();
			if ( level < 0 )
			{
				throw new Exception("Forbidden value on level = " + level + ", it doesn't respect the following condition : level < 0");
			}
		}
	}
}
