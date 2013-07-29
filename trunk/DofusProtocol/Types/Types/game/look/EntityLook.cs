

// Generated on 07/29/2013 23:08:50
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class EntityLook
    {
        public const short Id = 55;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public short bonesId;
        public IEnumerable<short> skins;
        public IEnumerable<int> indexedColors;
        public IEnumerable<short> scales;
        public IEnumerable<Types.SubEntity> subentities;
        
        public EntityLook()
        {
        }
        
        public EntityLook(short bonesId, IEnumerable<short> skins, IEnumerable<int> indexedColors, IEnumerable<short> scales, IEnumerable<Types.SubEntity> subentities)
        {
            this.bonesId = bonesId;
            this.skins = skins;
            this.indexedColors = indexedColors;
            this.scales = scales;
            this.subentities = subentities;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteShort(bonesId);
            writer.WriteUShort((ushort)skins.Count());
            foreach (var entry in skins)
            {
                 writer.WriteShort(entry);
            }
            writer.WriteUShort((ushort)indexedColors.Count());
            foreach (var entry in indexedColors)
            {
                 writer.WriteInt(entry);
            }
            writer.WriteUShort((ushort)scales.Count());
            foreach (var entry in scales)
            {
                 writer.WriteShort(entry);
            }
            writer.WriteUShort((ushort)subentities.Count());
            foreach (var entry in subentities)
            {
                 entry.Serialize(writer);
            }
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            bonesId = reader.ReadShort();
            if (bonesId < 0)
                throw new Exception("Forbidden value on bonesId = " + bonesId + ", it doesn't respect the following condition : bonesId < 0");
            var limit = reader.ReadUShort();
            skins = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 (skins as short[])[i] = reader.ReadShort();
            }
            limit = reader.ReadUShort();
            indexedColors = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 (indexedColors as int[])[i] = reader.ReadInt();
            }
            limit = reader.ReadUShort();
            scales = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 (scales as short[])[i] = reader.ReadShort();
            }
            limit = reader.ReadUShort();
            subentities = new Types.SubEntity[limit];
            for (int i = 0; i < limit; i++)
            {
                 (subentities as Types.SubEntity[])[i] = new Types.SubEntity();
                 (subentities as Types.SubEntity[])[i].Deserialize(reader);
            }
        }
        
        public virtual int GetSerializationSize()
        {
            return sizeof(short) + sizeof(short) + skins.Sum(x => sizeof(short)) + sizeof(short) + indexedColors.Sum(x => sizeof(int)) + sizeof(short) + scales.Sum(x => sizeof(short)) + sizeof(short) + subentities.Sum(x => x.GetSerializationSize());
        }
        
    }
    
}