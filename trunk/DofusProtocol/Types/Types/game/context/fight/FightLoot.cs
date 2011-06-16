// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'FightLoot.xml' the '14/06/2011 11:32:46'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
	public class FightLoot
	{
		public const uint Id = 41;
		public short TypeId
		{
			get
			{
				return 41;
			}
		}
		
		public short[] objects;
		public int kamas;
		
		public FightLoot()
		{
		}
		
		public FightLoot(short[] objects, int kamas)
		{
			this.objects = objects;
			this.kamas = kamas;
		}
		
		public virtual void Serialize(IDataWriter writer)
		{
			writer.WriteUShort((ushort)objects.Length);
			for (int i = 0; i < objects.Length; i++)
			{
				writer.WriteShort(objects[i]);
			}
			writer.WriteInt(kamas);
		}
		
		public virtual void Deserialize(IDataReader reader)
		{
			int limit = reader.ReadUShort();
			objects = new short[limit];
			for (int i = 0; i < limit; i++)
			{
				objects[i] = reader.ReadShort();
			}
			kamas = reader.ReadInt();
			if ( kamas < 0 )
			{
				throw new Exception("Forbidden value on kamas = " + kamas + ", it doesn't respect the following condition : kamas < 0");
			}
		}
	}
}
