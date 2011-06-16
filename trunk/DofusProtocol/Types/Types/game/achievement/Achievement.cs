// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'Achievement.xml' the '14/06/2011 11:32:44'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
	public class Achievement
	{
		public const uint Id = 363;
		public short TypeId
		{
			get
			{
				return 363;
			}
		}
		
		public short id;
		
		public Achievement()
		{
		}
		
		public Achievement(short id)
		{
			this.id = id;
		}
		
		public virtual void Serialize(IDataWriter writer)
		{
			writer.WriteShort(id);
		}
		
		public virtual void Deserialize(IDataReader reader)
		{
			id = reader.ReadShort();
			if ( id < 0 )
			{
				throw new Exception("Forbidden value on id = " + id + ", it doesn't respect the following condition : id < 0");
			}
		}
	}
}
