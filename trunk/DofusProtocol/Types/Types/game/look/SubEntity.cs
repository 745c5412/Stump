// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'SubEntity.xml' the '14/06/2011 11:32:50'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
	public class SubEntity
	{
		public const uint Id = 54;
		public short TypeId
		{
			get
			{
				return 54;
			}
		}
		
		public byte bindingPointCategory;
		public byte bindingPointIndex;
		public Types.EntityLook subEntityLook;
		
		public SubEntity()
		{
		}
		
		public SubEntity(byte bindingPointCategory, byte bindingPointIndex, Types.EntityLook subEntityLook)
		{
			this.bindingPointCategory = bindingPointCategory;
			this.bindingPointIndex = bindingPointIndex;
			this.subEntityLook = subEntityLook;
		}
		
		public virtual void Serialize(IDataWriter writer)
		{
			writer.WriteByte(bindingPointCategory);
			writer.WriteByte(bindingPointIndex);
			subEntityLook.Serialize(writer);
		}
		
		public virtual void Deserialize(IDataReader reader)
		{
			bindingPointCategory = reader.ReadByte();
			if ( bindingPointCategory < 0 )
			{
				throw new Exception("Forbidden value on bindingPointCategory = " + bindingPointCategory + ", it doesn't respect the following condition : bindingPointCategory < 0");
			}
			bindingPointIndex = reader.ReadByte();
			if ( bindingPointIndex < 0 )
			{
				throw new Exception("Forbidden value on bindingPointIndex = " + bindingPointIndex + ", it doesn't respect the following condition : bindingPointIndex < 0");
			}
			subEntityLook = new Types.EntityLook();
			subEntityLook.Deserialize(reader);
		}
	}
}
