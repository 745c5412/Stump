// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'InteractiveElement.xml' the '30/06/2011 11:40:24'
using System;
using Stump.Core.IO;
using System.Collections.Generic;
using System.Linq;

namespace Stump.DofusProtocol.Types
{
	public class InteractiveElement
	{
		public const uint Id = 80;
		public virtual short TypeId
		{
			get
			{
				return 80;
			}
		}
		
		public int elementId;
		public int elementTypeId;
		public IEnumerable<Types.InteractiveElementSkill> enabledSkills;
		public IEnumerable<Types.InteractiveElementSkill> disabledSkills;
		
		public InteractiveElement()
		{
		}
		
		public InteractiveElement(int elementId, int elementTypeId, IEnumerable<Types.InteractiveElementSkill> enabledSkills, IEnumerable<Types.InteractiveElementSkill> disabledSkills)
		{
			this.elementId = elementId;
			this.elementTypeId = elementTypeId;
			this.enabledSkills = enabledSkills;
			this.disabledSkills = disabledSkills;
		}
		
		public virtual void Serialize(IDataWriter writer)
		{
			writer.WriteInt(elementId);
			writer.WriteInt(elementTypeId);
			writer.WriteUShort((ushort)enabledSkills.Count());
			foreach (var entry in enabledSkills)
			{
				writer.WriteShort(entry.TypeId);
				entry.Serialize(writer);
			}
			writer.WriteUShort((ushort)disabledSkills.Count());
			foreach (var entry in disabledSkills)
			{
				writer.WriteShort(entry.TypeId);
				entry.Serialize(writer);
			}
		}
		
		public virtual void Deserialize(IDataReader reader)
		{
			elementId = reader.ReadInt();
			if ( elementId < 0 )
			{
				throw new Exception("Forbidden value on elementId = " + elementId + ", it doesn't respect the following condition : elementId < 0");
			}
			elementTypeId = reader.ReadInt();
			int limit = reader.ReadUShort();
			enabledSkills = new Types.InteractiveElementSkill[limit];
			for (int i = 0; i < limit; i++)
			{
				(enabledSkills as Types.InteractiveElementSkill[])[i] = ProtocolTypeManager.GetInstance<Types.InteractiveElementSkill>(reader.ReadShort());
				(enabledSkills as Types.InteractiveElementSkill[])[i].Deserialize(reader);
			}
			limit = reader.ReadUShort();
			disabledSkills = new Types.InteractiveElementSkill[limit];
			for (int i = 0; i < limit; i++)
			{
				(disabledSkills as Types.InteractiveElementSkill[])[i] = ProtocolTypeManager.GetInstance<Types.InteractiveElementSkill>(reader.ReadShort());
				(disabledSkills as Types.InteractiveElementSkill[])[i].Deserialize(reader);
			}
		}
	}
}
